# IMS CLR 1.0 Reference Implementation

Comprehensive Learner Record (CLR) is an IMS specification for transmitting assertions of learning achievements using a REST API. The specification describes 2 kinds of CLR applications: Consumers and Providers. Consumers initiate REST API calls. Providers respond to the REST API calls from Consumers. An application can be both a Consumer and a Provider.

This standard does not specify how applications store or process the CLR data, nor does it specify how applications should visualize CLR data.

This repo has several uses:
 * Allows a developer to examine source code and see how something works.
 * Allows IMS a chance to have a working application that helps us validate the specs and work through the use cases.
 * Allows a developer to run the reference implementation locally for developing their own solutions against it.
 * Will be continually updated as specs change, new versions come out etc.

## Spec Locations

The CLR specification is under development in the [ComprehensiveLearnerRecord](https://github.com/IMSGlobal/ComprehensiveLearnerRecord) GitHub repository. It recently reached Candidate Final Public status.

## Contents of this Repository

This repository has 3 reference implementions: a Consumer, a Provider, and an Authorization Server. All three reference implementations are ASP.NET Core applications that can be run on Linux or Windows.

* The Consumer app has a UI that allows the user to: 1) dynamically register with a Provider/Authorization Server combo (can be any CLR Provider, not just the reference implementation here), 2) send API requests to the Provider, and 3) display the results.
* The Provider app has a Swagger UI to explore the CLR REST API, but mostly it: 1) receives API requests from Consumers and 2) returns the results.
* The Authorization Server provides all the OAuth 2.0 endpoints used by the Consumer and Provider: _register_, _authorize_, and _token_. This reference implementation is built on [IdentityServer4](https://github.com/IdentityServer/IdentityServer4) with a custom controller to support the [RFC7591](https://tools.ietf.org/html/rfc7591) OAuth 2.0 Dynamic Client Registration endpoint. You can use your own authorization server with the Provider reference implementation by changing the _appsettings.json_ file in the ClrProvider deployment directory. See [Deployment Instructions](#deployment-instructions) for details.

## Source Code

Where to find points of interest in the reference implementation source code:

```
/CLR-reference-implementation/
+-- ClrAuth/src/ (source code for the authorization server)
    +-- Controllers/ (source code for the RegistrationController which handles RFC7591 OAuth 2.0 Dynamic Client Registration)
+-- ClrConsumer/src/ (source code for the Consumer app)
    +-- Models/ (UI data models)
    +-- Pages/Registration/ (UI for the registration process - for example, Register.cshtml.cs calls the Registration endpoint in authorization server)
    +-- Pages/Service/ (UI for the service calls - for example, GetClrs.cshtml.cs calls the GET CLRs endpoint in Provider server)
+-- ClrProvider/src/ (source code for the Provider app)
    +-- /Controllers/ (based on generated controllers from the OpenAPI file with added conforming behavior for reference)
+-- ClrLibrary/src/
    +-- /Models/ (generated data models from the OpenAPI file)
```

## Deployment Instructions

- [Deploy to Linux](deploy-linux.md)
- [Deploy to Windows](deploy-windows.md)

## License

This reference implementation is released under a <b>modified</b> Apache 2.0 license that
can be found in the root directory of this repository.
