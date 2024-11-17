# Cloud Database Structure

## Azure SQL Database
Purpose
Store structured data related to properties, customers, mortgage applications, and financial information.

Tables
**House**: Property details.

HouseID (Primary Key): Unique identifier for each property.
Location: Property address or location.
Price: Listing price of the house.
Bedrooms: Number of bedrooms.
Bathrooms: Number of bathrooms.
Garden (Boolean): Indicates if there is a garden.
Balcony (Boolean): Indicates if there is a balcony.
EnergyClass: Energy efficiency rating of the house.

**Customer**: Customer details.

CustomerID (Primary Key): Unique identifier for each customer.
Name: Customer’s full name.
Email: Customer’s email address.
PhoneNumber: Customer’s phone number.
FinancialInformationID (Foreign Key): Links to financial information.

**FinancialInformation**: Customer financial records.

FinancialInformationID (Primary Key): Unique identifier for each financial record.
Income: Customer’s income.
CreditScore: Customer’s credit score.

## Azure Blob Storage
Purpose
Store unstructured data such as property images and mortgage offer documents.

Containers
**images**: Stores property images, referenced by HouseID.

Naming Convention: house_<HouseID>_<ImageNumber>.jpg
Metadata: HouseID for easy retrieval.

**mortgage-offers**: Stores mortgage offer documents (e.g., PDF).

Naming Convention: <autogenrated>.pdf
Metadata:

## Azure Table Storage
Purpose
Efficient access to mortgage application statuses and offer tracking.

Tables
**Applications**: Contains all necessary data related to each mortgage application.

PartitionKey: CustomerID (Groups all applications by customer).
RowKey: ApplicationID (Unique identifier for each application).
Attributes:
HouseID: ID of the house the mortgage is for.
ApplicationDate: Date when the application was submitted.
Status: Current status of the application (e.g., "submitted," "in review," "approved").
Income: Customer’s income for application reference.
CreditScore: Customer’s credit score.
MortgageOfferID: Links to the Offers table if an offer is generated.
Timestamp: Date and time of the most recent status update.
CustomerEmail: Customer's email for communication.
OfferUrl:

**Offers**: Stores information about each mortgage offer and its status.

PartitionKey: CustomerID (Groups all offers by customer).
RowKey: MortgageOfferID (Unique identifier for each mortgage offer).
Attributes: Link to the mortgage offer document.
CustomerEmial: Customer's email for communication.
Status: Current status of the offer (e.g., "sent," "viewed," "expired").
ExpirationDate: Date and time when the offer expires.
OfferURL: URL to the document stored in Blob Storage.
Timestamp: Date and time when the offer was created or last updated.