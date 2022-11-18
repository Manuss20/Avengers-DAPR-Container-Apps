param name string
param location string = resourceGroup().location
param containerAppEnviromentId string
param repositoryImage string = 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest'
param envVars array = []
param minReplicas int = 1
param maxReplicas int = 1
param port int = 80
param externalIngress bool = false
param allowInsecure bool = true
param transport string = 'http'
param appProtocol string = 'http'
param uaiId string

resource containerApp 'Microsoft.App/containerApps@2022-03-01' = {
  name: name
  location: location
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${uaiId}': {}
    }
  }
  properties: {
    managedEnvironmentId: containerAppEnviromentId
    configuration: {
      dapr: {
        enabled: true
        appId: name
        appPort: port
        appProtocol: appProtocol
      }
      activeRevisionsMode: 'single'
      secrets: [
      ]
      registries: [
      ]
      ingress: {
        external: externalIngress
        targetPort: port
        allowInsecure: allowInsecure
        transport: transport
      }
    }
    template: {
      containers: [
        {
          image: repositoryImage
          name: name
          env: envVars
        }
      ]
      scale: {
        minReplicas: minReplicas
        maxReplicas: maxReplicas
      }
    }
  }
}

output fqdn string = containerApp.properties.configuration.ingress.fqdn
