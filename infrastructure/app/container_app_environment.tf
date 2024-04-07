resource "azurerm_container_app_environment" "main" {
  name                       = "cae-${local.resource-suffix-name}-${var.region-short}"
  location                   = azurerm_resource_group.main.location
  resource_group_name        = azurerm_resource_group.main.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.main.id
}