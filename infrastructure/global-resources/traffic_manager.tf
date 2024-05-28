resource "azurerm_public_ip" "main" {
  name                = "pip-${local.resource-suffix-name}"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  allocation_method   = "Static"
  domain_name_label   = local.resource-suffix-name
}


resource "azurerm_traffic_manager_profile" "main" {
  name                   = "traf-${local.resource-suffix-name}"
  resource_group_name    = azurerm_resource_group.main.name
  traffic_routing_method = "Geographic"

  dns_config {
    relative_name = local.resource-suffix-name
    ttl           = 100
  }

  monitor_config {
    protocol                     = "HTTPS"
    port                         = 443
    path                         = "/monitoring/readiness"
    interval_in_seconds          = 30
    timeout_in_seconds           = 10
    tolerated_number_of_failures = 3
  }

}
