FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 44360
ENV ASPNETCORE_URLS=http://+:44360

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /buildprocess

# Install Node.js
ENV NODE_VERSION=24.4.0
ENV NODE_DOWNLOAD_URL=https://nodejs.org/dist/v$NODE_VERSION/node-v$NODE_VERSION-linux-x64.tar.gz
RUN curl -SL "$NODE_DOWNLOAD_URL" --output nodejs.tar.gz \
    && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
    && rm nodejs.tar.gz \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs

# Install Yarn
ENV YARN_VERSION=1.22.22
RUN npm install -g yarn@$YARN_VERSION

# Copy everything needed for build (all source, solution, and common.props)
COPY . .


WORKDIR /buildprocess/src/ChatUapp.Web

RUN dotnet dev-certs https -v -ep openiddict.pfx -p 58922e2f-ffbf-4d2b-9280-f30732b5f35a

# Install ABP CLI and client-side libs
RUN dotnet tool install -g Volo.Abp.Cli
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN abp install-libs

# Build and publish
RUN dotnet publish "/buildprocess/src/ChatUapp.Web/ChatUapp.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ChatUapp.Web.dll"]