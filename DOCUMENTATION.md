# Database Cloud Structure

## Azure SQL Database
Purpose: Store structured data related to properties, customers, and financial records.
Tables:
**Properties**: Includes property details (ID, location, price, features).
**Customers**: Stores customer information (ID, name, email, contact).
**FinancialInformation**: Holds customer financial records (Income, credit score).

## Azure Blob Storage
Purpose: Store unstructured data, including property images and mortgage documents.
Containers:
**images**: Stores images of properties referenced by PropertyID.
**mortgage-documents**: Stores mortgage documents referenced by CustomerID.

## Azure Table Storage
Purpose: Maintain mortgage application data and offer statuses efficiently.
Tables:
**MortgageApplications**: Key-value table for each mortgage application (status, timestamps).
**OfferStatus**: Key-value table to track mortgage offer statuses.