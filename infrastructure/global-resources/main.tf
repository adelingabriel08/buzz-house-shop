resource "azurerm_resource_group" "main" {
  name     = "rg-${local.resource-suffix-name}"
  location = local.location
}