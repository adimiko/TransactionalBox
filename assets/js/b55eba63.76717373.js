"use strict";(self.webpackChunkdocumentation=self.webpackChunkdocumentation||[]).push([[240],{1286:(e,t,n)=>{n.r(t),n.d(t,{assets:()=>c,contentTitle:()=>o,default:()=>p,frontMatter:()=>s,metadata:()=>i,toc:()=>l});var a=n(4848),r=n(8453);const s={sidebar_position:2},o="Apache Kafka",i={id:"transport/kafka",title:"Apache Kafka",description:"Install package",source:"@site/docs/transport/kafka.md",sourceDirName:"transport",slug:"/transport/kafka",permalink:"/docs/transport/kafka",draft:!1,unlisted:!1,editUrl:"https://github.com/adimiko/TransactionalBox/tree/main/documentation/docs/transport/kafka.md",tags:[],version:"current",sidebarPosition:2,frontMatter:{sidebar_position:2},sidebar:"tutorialSidebar",previous:{title:"InMemory",permalink:"/docs/transport/in-memory"},next:{title:"Simple Sample",permalink:"/docs/samples/simple-sample"}},c={},l=[{value:"Install package",id:"install-package",level:2},{value:"Outbox",id:"outbox",level:2},{value:"Register",id:"register",level:3},{value:"Inbox",id:"inbox",level:2},{value:"Register",id:"register-1",level:3}];function d(e){const t={code:"code",h1:"h1",h2:"h2",h3:"h3",pre:"pre",...(0,r.R)(),...e.components};return(0,a.jsxs)(a.Fragment,{children:[(0,a.jsx)(t.h1,{id:"apache-kafka",children:"Apache Kafka"}),"\n",(0,a.jsx)(t.h2,{id:"install-package",children:"Install package"}),"\n",(0,a.jsx)(t.pre,{children:(0,a.jsx)(t.code,{className:"language-csharp",children:"dotnet add package TransactionalBox.Kafka\n"})}),"\n",(0,a.jsx)(t.h2,{id:"outbox",children:"Outbox"}),"\n",(0,a.jsx)(t.h3,{id:"register",children:"Register"}),"\n",(0,a.jsx)(t.pre,{children:(0,a.jsx)(t.code,{className:"language-csharp",children:"builder.Services.AddTransactionalBox(x =>\n{\n    x.AddOutbox(\n        storage => ...,\n        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers)\n    );\n});\n\n"})}),"\n",(0,a.jsx)(t.h2,{id:"inbox",children:"Inbox"}),"\n",(0,a.jsx)(t.h3,{id:"register-1",children:"Register"}),"\n",(0,a.jsx)(t.pre,{children:(0,a.jsx)(t.code,{className:"language-csharp",children:"builder.Services.AddTransactionalBox(x =>\n{\n    x.AddInbox(\n        storage => ...,\n        transport => transport.UseKafka(settings => settings.BootstrapServers = bootstrapServers)\n    );\n});\n\n"})})]})}function p(e={}){const{wrapper:t}={...(0,r.R)(),...e.components};return t?(0,a.jsx)(t,{...e,children:(0,a.jsx)(d,{...e})}):d(e)}},8453:(e,t,n)=>{n.d(t,{R:()=>o,x:()=>i});var a=n(6540);const r={},s=a.createContext(r);function o(e){const t=a.useContext(s);return a.useMemo((function(){return"function"==typeof e?e(t):{...t,...e}}),[t,e])}function i(e){let t;return t=e.disableParentContext?"function"==typeof e.components?e.components(r):e.components||r:o(e.components),a.createElement(s.Provider,{value:t},e.children)}}}]);