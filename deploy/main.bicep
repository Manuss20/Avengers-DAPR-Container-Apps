param location string = resourceGroup().location

var acrPullRole = resourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d')

// Creation of the Azure Container Registry
resource acr 'Microsoft.ContainerRegistry/registries@2021-09-01' = {
  name: toLower('${resourceGroup().name}')
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
    publicNetworkAccess: 'Enabled'
  }
}

resource uai 'Microsoft.ManagedIdentity/userAssignedIdentities@2022-01-31-preview' = {
  name: 'id-avengers'
  location: location
}

@description('This allows the managed identity of the container app to access the registry, note scope is applied to the wider ResourceGroup not the ACR')
resource uaiRbac 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(resourceGroup().id, uai.id, acrPullRole)
  properties: {
    roleDefinitionId: acrPullRole
    principalId: uai.properties.principalId
    principalType: 'ServicePrincipal'
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
    uaiId: uai.id
    containerAppEnviromentId: env.outputs.id
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
    uaiId: uai.id
    containerAppEnviromentId: env.outputs.id
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
    uaiId: uai.id
    containerAppEnviromentId: env.outputs.id
    registry: acr.name
    envVars: shared_config
    externalIngress: true
  }
}
