using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Services.Constants
{
    public static class VerityConstants
    {
        public const string ConnectionRequestReceived = "request-received";
        public const string ConnectionResponseSent = "response-sent";
        public const string CredentialDefinitionWriteStatusReport = "status-report";
        public const string IssueCredentialOfferSent = "sent";
        public const string IssueCredentialSent = "sent";
        public const string IssuerCreated = "public-identifier-created";
        public const string IssuerIdentifier = "public-identifier";
        public const string IssuerIdentifierProblem = "problem-report";
        public const string RelationshipInvitation = "invitation";
        public const string RelationshipRequestCreated = "created";
        public const string SchemaWriteStatusReport = "status-report";
    }

    public static class VerityMessageAttributes
    {
        public const string TypeAttribute = "@type";

    }

    public static class VerityMessageFamilies
    {
        public const string ConnectionRequestReceived = "did:sov:BzCbsNYhMrjHiqZDTUASHg;spec/connections/1.0/request-received";
        public const string ConnectionResponseSent = "did:sov:BzCbsNYhMrjHiqZDTUASHg;spec/connections/1.0/response-sent";
        public const string CreateIssuerCreated = "did:sov:123456789abcdefghi1234;spec/issuer-setup/0.6/public-identifier-created";
        public const string IssueCredentialAckReceived = "did:sov:BzCbsNYhMrjHiqZDTUASHg;spec/issue-credential/1.0/ack-received";
        public const string IssueCredentialStatusReport = "did:sov:BzCbsNYhMrjHiqZDTUASHg;spec/issue-credential/1.0/status-report";
        public const string IssuerProblemReport = "did:sov:123456789abcdefghi1234;spec/issuer-setup/0.6/problem-report";
        public const string PublicIdentifier = "did:sov:123456789abcdefghi1234;spec/issuer-setup/0.6/public-identifier";
        public const string RelationshipCreated = "did:sov:123456789abcdefghi1234;spec/relationship/1.0/created";
        public const string RelationshipInvitation = "did:sov:123456789abcdefghi1234;spec/relationship/1.0/invitation";
        public const string SentIssueCredentialMessage = "did:sov:BzCbsNYhMrjHiqZDTUASHg;spec/issue-credential/1.0/sent";
        public const string UpdateConfigsResponse = "did:sov:123456789abcdefghi1234;spec/update-configs/0.6/status-report";
        public const string UpdateEndpointsResponse = "did:sov:123456789abcdefghi1234;spec/configs/0.6/COM_METHOD_UPDATED";
        public const string WriteCredentialDefinitionProblem = "did:sov:123456789abcdefghi1234;spec/write-cred-def/0.6/problem-report";
        public const string WriteCredentialDefinitionResponse = "did:sov:123456789abcdefghi1234;spec/write-cred-def/0.6/status-report";
        public const string WriteSchemaProblem = "did:sov:123456789abcdefghi1234;spec/write-schema/0.6/problem-report";
        public const string WriteSchemaResponse = "did:sov:123456789abcdefghi1234;spec/write-schema/0.6/status-report";
    }
}
