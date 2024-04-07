resource "azurerm_cosmosdb_account" "main" {
  name                      = "cosno-${local.resource-suffix-name}"
  location                  = local.location
  resource_group_name       = azurerm_resource_group.main.name
  offer_type                = "Standard"
  kind                      = "GlobalDocumentDB"
  enable_automatic_failover = true
  enable_multiple_write_locations = true

  geo_location {
    location          = local.location
    failover_priority = 0
  }

  geo_location {
    location          = local.secondary_location
    failover_priority = 1
  }

  consistency_policy {
    consistency_level       = "BoundedStaleness"
    max_interval_in_seconds = 300
    max_staleness_prefix    = 100000
  }

  depends_on = [
    azurerm_resource_group.main
  ]
}

resource "azurerm_cosmosdb_sql_database" "main" {
  name                = azurerm_cosmosdb_account.main.name
  resource_group_name = azurerm_resource_group.main.name
  account_name        = azurerm_cosmosdb_account.main.name
  autoscale_settings {
    max_throughput = 4000
  }
}
