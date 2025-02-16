"use strict";(self.webpackChunkdocumentation=self.webpackChunkdocumentation||[]).push([[670],{5230:(e,n,s)=>{s.r(n),s.d(n,{assets:()=>l,contentTitle:()=>d,default:()=>x,frontMatter:()=>r,metadata:()=>o,toc:()=>a});var i=s(4848),t=s(8453);const r={sidebar_position:4},d="Inbox",o={id:"inbox",title:"Inbox",description:"The inbox is responsible for getting messages from the transport and adding them to the storage, and then processes these messages.",source:"@site/docs/inbox.md",sourceDirName:".",slug:"/inbox",permalink:"/TransactionalBox/docs/inbox",draft:!1,unlisted:!1,editUrl:"https://github.com/adimiko/TransactionalBox/tree/main/documentation/docs/inbox.md",tags:[],version:"current",sidebarPosition:4,frontMatter:{sidebar_position:4},sidebar:"tutorialSidebar",previous:{title:"Outbox",permalink:"/TransactionalBox/docs/outbox"},next:{title:"InMemory",permalink:"/TransactionalBox/docs/storage/in-memory"}},l={},a=[{value:"Inbox Message",id:"inbox-message",level:3},{value:"Inbox Definition",id:"inbox-definition",level:3},{value:"Handling message in inbox handler",id:"handling-message-in-inbox-handler",level:3},{value:"Storage",id:"storage",level:2},{value:"Transport",id:"transport",level:2},{value:"Settings",id:"settings",level:2},{value:"AddMessagesToInboxSettings",id:"addmessagestoinboxsettings",level:3},{value:"CleanUpInboxSettings",id:"cleanupinboxsettings",level:3},{value:"CleanUpIdempotencyKeysSettings",id:"cleanupidempotencykeyssettings",level:3},{value:"ConfigureDeserialization",id:"configuredeserialization",level:3}];function c(e){const n={admonition:"admonition",br:"br",code:"code",h1:"h1",h2:"h2",h3:"h3",p:"p",pre:"pre",...(0,t.R)(),...e.components};return(0,i.jsxs)(i.Fragment,{children:[(0,i.jsx)(n.h1,{id:"inbox",children:"Inbox"}),"\n",(0,i.jsx)(n.p,{children:"The inbox is responsible for getting messages from the transport and adding them to the storage, and then processes these messages."}),"\n",(0,i.jsx)(n.admonition,{type:"tip",children:(0,i.jsx)(n.p,{children:"You can run inbox as a standalone service. You add reference to shared assemblies, but deploy as separate services.\nThis way, processing in inbox does not affect the performance of your service."})}),"\n",(0,i.jsx)(n.pre,{children:(0,i.jsx)(n.code,{className:"language-csharp",children:'builder.Services.AddTransactionalBox(x =>\n{\n    x.AddInbox(\n        storage => ...,\n        transport => ...,\n        settings => ...,\n        assembly => ...\n    );\n},\nsettings => settings.ServiceId = "ServiceName");\n'})}),"\n",(0,i.jsx)(n.h3,{id:"inbox-message",children:"Inbox Message"}),"\n",(0,i.jsx)(n.admonition,{type:"info",children:(0,i.jsx)(n.p,{children:"Inbox message class name must be unique per service."})}),"\n",(0,i.jsx)(n.pre,{children:(0,i.jsx)(n.code,{className:"language-csharp",children:"public class CreateCustomerCommandMessage : InboxMessage\n{\n    public Guid Id { get; init; }\n\n    public string FirstName { get; init; }\n\n    public string LastName { get; init; }\n\n    public int Age { get; init; }\n}\n"})}),"\n",(0,i.jsx)(n.h3,{id:"inbox-definition",children:"Inbox Definition"}),"\n",(0,i.jsx)(n.admonition,{type:"info",children:(0,i.jsxs)(n.p,{children:["You do not need to define a inbox definition for each message.",(0,i.jsx)(n.br,{}),"\n","Listens by default for incoming messages to the current service (does not listen on events).",(0,i.jsx)(n.br,{}),"\n","If you want to listen for events, you need to define the ",(0,i.jsx)(n.code,{children:"PublishedBy"})," property."]})}),"\n",(0,i.jsx)(n.pre,{children:(0,i.jsx)(n.code,{className:"language-csharp",children:'public class CreatedCustomerEventMessageDefinition : InboxDefinition<CreatedCustomerEventMessage>\n{\n    public CreatedCustomerEventMessageDefinition() \n    {\n        PublishedBy = "Customers";\n    }\n}\n'})}),"\n",(0,i.jsx)(n.h3,{id:"handling-message-in-inbox-handler",children:"Handling message in inbox handler"}),"\n",(0,i.jsx)(n.admonition,{type:"info",children:(0,i.jsxs)(n.p,{children:["Each ",(0,i.jsx)(n.code,{children:"InboxMessage"})," must have a ",(0,i.jsx)(n.code,{children:"IInboxHandler"})," declared."]})}),"\n",(0,i.jsx)(n.pre,{children:(0,i.jsx)(n.code,{className:"language-csharp",children:"public class CreatedCustomerEventMessageHandler : IInboxHandler<CreatedCustomerEventMessage>\n{\n    ...\n    \n    public async Task Handle(CreatedCustomerEventMessage message, IExecutionContext executionContext)\n    {\n        // Your logic\n    }\n}\n"})}),"\n",(0,i.jsx)(n.h2,{id:"storage",children:"Storage"}),"\n",(0,i.jsxs)("table",{children:[(0,i.jsx)("tr",{children:(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Options"})})}),(0,i.jsx)("tr",{children:(0,i.jsx)("td",{children:"In Memory (Default)"})}),(0,i.jsx)("tr",{children:(0,i.jsx)("td",{children:"Entity Framework Core (Relational)"})})]}),"\n",(0,i.jsx)(n.h2,{id:"transport",children:"Transport"}),"\n",(0,i.jsxs)("table",{children:[(0,i.jsx)("tr",{children:(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Options"})})}),(0,i.jsx)("tr",{children:(0,i.jsx)("td",{children:"In Memory (Default)"})}),(0,i.jsx)("tr",{children:(0,i.jsx)("td",{children:"Apache Kafka "})})]}),"\n",(0,i.jsx)(n.h2,{id:"settings",children:"Settings"}),"\n",(0,i.jsx)(n.h3,{id:"addmessagestoinboxsettings",children:"AddMessagesToInboxSettings"}),"\n",(0,i.jsxs)("table",{children:[(0,i.jsxs)("tr",{children:[(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Name"})}),(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Type"})}),(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Default Value"})}),(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Description"})})]}),(0,i.jsxs)("tr",{children:[(0,i.jsx)("td",{children:"DefaultTimeToLiveIdempotencyKey"}),(0,i.jsx)("td",{children:"TimeSpan"}),(0,i.jsx)("td",{children:"7 days"}),(0,i.jsx)("td",{children:"Default time to live idempotency key. After the time expires, the key will be removed."})]})]}),"\n",(0,i.jsx)(n.h3,{id:"cleanupinboxsettings",children:"CleanUpInboxSettings"}),"\n",(0,i.jsxs)("table",{children:[(0,i.jsxs)("tr",{children:[(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Name"})}),(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Type"})}),(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Default Value"})}),(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Description"})})]}),(0,i.jsxs)("tr",{children:[(0,i.jsx)("td",{children:"MaxBatchSize"}),(0,i.jsx)("td",{children:"int"}),(0,i.jsx)("td",{children:"10000"}),(0,i.jsx)("td",{children:"Maximum number of messages to clean up per job."})]}),(0,i.jsxs)("tr",{children:[(0,i.jsx)("td",{children:"IsEnabled"}),(0,i.jsx)("td",{children:"bool"}),(0,i.jsx)("td",{children:"true"}),(0,i.jsx)("td",{children:"Value responsible for whether the cleaning process is enabled."})]})]}),"\n",(0,i.jsx)(n.h3,{id:"cleanupidempotencykeyssettings",children:"CleanUpIdempotencyKeysSettings"}),"\n",(0,i.jsxs)("table",{children:[(0,i.jsxs)("tr",{children:[(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Name"})}),(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Type"})}),(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Default Value"})}),(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Description"})})]}),(0,i.jsxs)("tr",{children:[(0,i.jsx)("td",{children:"MaxBatchSize"}),(0,i.jsx)("td",{children:"int"}),(0,i.jsx)("td",{children:"10000"}),(0,i.jsx)("td",{children:"Maximum number of idepotency keys to clean up per job."})]}),(0,i.jsxs)("tr",{children:[(0,i.jsx)("td",{children:"Period"}),(0,i.jsx)("td",{children:"TimeSpan"}),(0,i.jsx)("td",{children:"1 hour"}),(0,i.jsx)("td",{children:"Time interval between cleaning idepotency keys."})]})]}),"\n",(0,i.jsx)(n.h3,{id:"configuredeserialization",children:"ConfigureDeserialization"}),"\n",(0,i.jsxs)("table",{children:[(0,i.jsxs)("tr",{children:[(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Name"})}),(0,i.jsx)("td",{children:(0,i.jsx)("b",{children:"Extension"})})]}),(0,i.jsxs)("tr",{children:[(0,i.jsx)("td",{children:"System.Text.Json (Default)"}),(0,i.jsx)("td",{children:"UseSystemTextJson()"})]})]})]})}function x(e={}){const{wrapper:n}={...(0,t.R)(),...e.components};return n?(0,i.jsx)(n,{...e,children:(0,i.jsx)(c,{...e})}):c(e)}},8453:(e,n,s)=>{s.d(n,{R:()=>d,x:()=>o});var i=s(6540);const t={},r=i.createContext(t);function d(e){const n=i.useContext(r);return i.useMemo((function(){return"function"==typeof e?e(n):{...n,...e}}),[n,e])}function o(e){let n;return n=e.disableParentContext?"function"==typeof e.components?e.components(t):e.components||t:d(e.components),i.createElement(r.Provider,{value:n},e.children)}}}]);