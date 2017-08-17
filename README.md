# Explore the Cosmos (DB) Cross-Platform

This repository contains cross-platform .NET Core 2.0 code to populate and access an [Azure Cosmos DB](https://docs.microsoft.com/azure/cosmos-db/introduction) database. It will actually work with any Mongo database. You have options to configure the end point, host, username, password, and database.

> **NOTICE!** This is a work in progress and is not final.

> **DISCLAIMER:**
> Use of Cosmos DB will incur charges against your Azure account. Please be aware of these charges and understand any activity against the Azure instance will result in fees. You can alternatively use a local emulator or connect to a Mongo database.

## Configuration

The applications that require configuration all prioritize, in order, environment variables then `appsettings.json`. This allows you to set the variables in the environment, or, if you are hosting the app, use the environment and/or app settings feature on the host to configure secrets. The variables and their explanation follows:

* USDA_PORT: the port to access the database (default for Cosmos DB is 10255)
* USDA_HOST: the host URL
* USDA_DATABASE: the database name
* USDA_USER: the database user
* USDA_PASSWORD: the database password

The connector uses SSL and assumes the credential is `SCRAM-SHA-1`.

## Database setup: `usda-importer`

Use this console tool to configure the collections and import data for the demo. It can take several minutes to run. The data is imported from the SR28 version of the [USDA National Nutrient Database for Standard Reference](https://www.ars.usda.gov/northeast-area/beltsville-md/beltsville-human-nutrition-research-center/nutrient-data-laboratory/docs/usda-national-nutrient-database-for-standard-reference/). The URLs for the data files are configured in `appsettings.json`.

The importer will load the data files, parse them into entities, correlate related entities, drop existing collections, create new collections, and import the data. Three collections are created:

* `foodGroups` is a list of categories for food items
* `nutrientDefinitions` is a list of nutrient data collected for each food item (i.e. Protein, Calories, etc.)
* `foodItems` is the master list of foods

## Projects

The following projects exist and reference each other through relative paths:

* `usda-connector`: common code for connecting to the database
* `usda-console-test`: simple test to ensure database connections work and data exists
* `usda-importer`: code to pull files from the USDA database site, parse them, and insert them into the database
* `usda-models`: common models for the data
* `usda-web-api`: Web API interface to browse the data