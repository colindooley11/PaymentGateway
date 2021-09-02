# PaymentGateway API 
Payment Gateway API code test for Checkout.com

The Payment Gateway meets the requirements (albeit simply) for: 
1. Capture card payment
2. Get Payment Details
3. Allow simulation of bank 

I have added a few extras too ;) - also in this readme I discuss some of the points I missed in the archictecture part of interview with my terribly straw man discussion of an architecture. 
I absolutely know a bunch of approaches to provide security, scalability, uptime, resiliency etc and was frustrated I never explained them! 

### Extras
 - CI Pipeline, GitHub Action to build, test and publish app (albeit mostly code-genned) 
 - Published API to Azure App
 - Idempotency (not header based - based on PaymentReference)
 - Extra swagger documentation (XML and fluent based)  (including some example data) 
 - External calls to Cosmos Database
 - Application insights logging (its configured and implicitly logging dependencies, requests and exceptions) 
 - Fluent Validation 
 - Api Key Authentication *The APIKey is sent as the ApiKey header with the value "Merchant-1"* (this can be done easily in Swagger UI)

### Would like to haves
 - Health check Action (i.e. route)  for Load Balancing, Health Checks for runtime deployment (.Net core allows liveness and readiness checks) 
 - Use of an orchastrator like Kubernetes to demo scaling 
 - Use of KeyVault (Secret management), and  UserSecrets (i.e. NET core user secret files) , Environment variables 
 - Containerisation (I started but then backed out not knowing if I would be able to get you guys running without scripts) 
 - Cosmos DB change feed and/ or Azure Service Bus integration to persist payment details in a materialized view/projection in a separate read store this is why I went with Cosmos as opposed to EF in memory as I was going to use containers and pull the Emulator container but it was 5GB in size

## Using the App on the Web
Please use the URI

` https://paymentgateway-api.azurewebsites.net/swagger/index.html`

You will then be presented with the UI to allow you to POST a Card Capture and GET Payment Details

The bank simulator supports 2  sets of cards: 

- *4444333322221111* - Provides success
- *Any other valid card number* - Provides decline 

## Running the application locally
### Pre-Requisites

The application uses .NET 5 so please ensure you have the latest SDK https://dotnet.microsoft.com/download/dotnet/5.0
I was *going* to use containers but I have not got a great working knowledge here,  but I appreciate this would open up other options (nice to haves discussed later) 

Once you have cloned the repo (or downloaded a package) the absolute easiest way is to 'F5' in Visual Studio,
this is easier as the Swagger UI is also loaded, however,  if this is not possible, then please do the following: 

please CD into:

  `[path to files]\PaymentGateway\PaymentGateway.Api\PaymentGateway.Api`

### Running
Once here, build and run the app

` dotnet run `

This should ensure packages are built (and dependencies restored) and the app should fire up and can then be hit from Postman (more on that in a bit)

The swagger UI can then be visited in either case (whether loaded automatically by F5'ing or using the following URI)

`https://localhost:5001/swagger/index.html`

## Testing
There are a number of tests
1. In and out of process "component tests"  - (Aligned with https://martinfowler.com/articles/microservice-testing/ (which we pretty much as a go to at ASOS)  *WebApplicationFactory* and - previously - TestServer alone allows us to get the API up in memory and allows us to start the TDD cycle by starting out adding test for missing routes (i.e the Actions on the controllers)  :) 

I always attempt to test sociably as opposed to in a solitary way.  The unit - here - is the API and I observe the input, outputs and colloborators on the API using spys, stubs and mocks.  Although both solitary and sociable testing are perfectly fine and can be used interchangably, at ASOS we have definitely favoured more sociable testing and testing as much behaviour as possible pushing out - right to the edges-  and stubbing the external calls only
2.  Integration tests - these test the Cosmos DB commands and queries used for the peristence and retrieval of payment data.  I note in one of the OutOfProcess tests that we could ditch these integration tests and use a "honeycomb" style approach and simply have the database called

If you CD into 

` [path to files]\PaymentGateway\PaymentGateway.Api` 

and run 

 dotnet test .\PaymentGatewayTest.sln


## Structure
The API has 2 controllers 
1. CardPaymentController - this offers a POST action where a CardPaymentRequest containing relevant fields and a unique ID (payment reference) to hang a payment on (in naive impl)   
2. PaymentDetailsController - this offers a GET action where a payment reference can be used to retrieve payment details 

In this naive implementation these live in the same host and process, but as discussed at the interview these could be sepearated into different APIs to allow different levels of scaling, network segregation, or hosting tech (orchastrator vs serverless etc).
Also what I never got to at the interview was the fact we generally use a multi region approach ( and one I would have suggested)  to help with uptime and resiliency, Load balancing via a Traffic Manager initally,  to route traffic to different regions and so the point here is that 2 requests may end up hitting VMs/containers in differet geo locations.  

For the bank simulator I coded only "Success" and "Failure".  I was more concerned with my approach to providing the stubbing/fake impl
I considered:
- middleware
- a stub server (getsandbox.com) for example
- stubbing the service in the API (I removed the IGateway abstraction and used a Typed client in the end)
- another host within the service
- building another API 
- a delegating handler which can be swapped out (and I have used on other prodution software I have developed) 

I chose the last! From a purist 12 factor app point of view having to meddle with config to accommodate different environments is not in keeping with some of its intents, however as the the changes are only in the Test host impl (although I do STILL have a stub in the main app) this is lessened to large degree 

The models are broken out simply to provide the ability to reference without too much coupling, I tend not to create too many projects if I can help and folders generally are good for organisation with namespaces to boot.

## Coding Style /Considerations 
The logic is very simple in the controllers, what little dependencies we have our injected in by correctly adding implementations in services etc. 

I have used a few mappers converting DTOs  from the data layer into the domain (if you can call it that) .  We used to be extremely strict about this at ASOS but this separation has lessend somewhat particuarly if the data doesnt change much.   Also we had a an absolutely nightmarish experience with AutoMapper which added the inability to read mapping without lots of study - completely wasteful.

A fair portion of effort always goes into TDDing and testing the code I produce as this has been drummed into me at ASOS

I must point out that I used BDDFy which I dont actually use that much. I saw this as an opportunity to see how flexible it was.  Although I was able to add examples  I felt that once a SUT becomes suitably multi-faceted and needs to be shaped up for different use cases that its too rigid. 
To this end I did all the tests with BDDFy for the CardPayment requirement and then purposely switched to Raw test fixtures in a BDD style for the PaymentDetails requirements

When considering the HttpStatus codes for the responses I had a peek at your API reference 

Lastly, most of this code has usings and constructors (unlike the code snippet I didn't spot it on) :)

## Known issues
I have hardcode the azure "host" in the Location URI returned on the 201 Created for a Card Payment this should be swapped out per environment



### THANKS
Thanks for the opportunity to do the code test hopefully you can see that I can actually code. 

Colin. 



