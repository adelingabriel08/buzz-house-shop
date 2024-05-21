resource "azurerm_resource_group" "main" {
  name     = "rg-${local.resource-suffix-name}-${var.region-short}"
  location = var.region
}