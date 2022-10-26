param location string = resourceGroup().location

// Creation of the Azure Container Registry
resource acr 'Microsoft.ContainerRegistry/registries@2021-09-01' = {
  name: toLower('${resourceGroup().name}-acr')
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
  }
}

// Creation of the Azure Container Environment
module env 'environment.bicep' = {
  name: 'containerAppEnviroment'
  params: {
    location: location
  }
}

var shared_config = [
  {
    name: 'ASPNETCORE_ENVIRONMENT'
    value: 'Development'
  }
  {
    name: 'APPINSIGHT_INSTRUMENTATIONKEY'
    value: env.outputs.appInsightsInstrumentationKey
  }
  {
    name: 'APPLICATIOINSIGHTS_CONNECTION_STRING'
    value: env.outputs.appInsightsConnectionString
  }
]

// Creation of the Azure Container Slot - Missions API
module missions 'container_app.bicep' = {
  name: 'missions'
  params: {
    name: 'missions'
    location: location
    registryPassword: acr.listCredentials().passwords[0].value
    registryUsername: acr.listCredentials().username
    containerAppEnvironmentId: env.outputs.id
    registry: acr.name
    envVars: shared_config
    externalIngress: false
  }
}

// Creation of the Azure Container Slot - Payment API
module payment 'container_app.bicep' = {
  name: 'payment'
  params: {
    name: 'payment'
    location: location
    registryPassword: acr.listCredentials().passwords[0].value
    registryUsername: acr.listCredentials().username
    containerAppEnvironmentId: env.outputs.id
    registry: acr.name
    envVars: shared_config
    externalIngress: false
  }
}

// Creation of the Azure Container Slot - Avengers
module avengers 'container_app.bicep' = {
  name: 'avengers'
  params: {
    name: 'avengers'
    location: location
    registryName: acr.listCredentials().username
    registryPassword: acr.listCredentials().passwords[0].value
    containerAppEnviroment: env.outputs.id
    registry: acr.name
    enVars: shared_config
    externalIngress: true
  }
}
