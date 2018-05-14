# parsle
Multi-platform transactional mail service wrapper for .net Core

Parsle is an attempt to start creating a wrapper for transactional mail services as there doesn't seem to be anything available to cope with the different API's providers like SendGrid, MailChimp and SendInBlue use (to name but a few).

The objective is initially to write a simple class library that accepts a standard mail object that can be passed to implementations of a mail service interface to convert that object into the ones required by the mail providers.  It would be great to get it to the point of auto-handling failures when sending mail to a given platform so that a failover mail service could be used. 

Any assistance greatfully received!
