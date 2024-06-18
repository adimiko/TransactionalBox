"use strict";(self.webpackChunkdocumentation=self.webpackChunkdocumentation||[]).push([[954],{7642:(e,n,o)=>{o.r(n),o.d(n,{assets:()=>s,contentTitle:()=>a,default:()=>x,frontMatter:()=>i,metadata:()=>d,toc:()=>l});var r=o(4848),t=o(8453);const i={sidebar_position:2},a="Entity Framework Core (Relational)",d={id:"storage/entity-framework-core",title:"Entity Framework Core (Relational)",description:"TransactionalBox is not responsible for configuring Entity Framework Core.",source:"@site/docs/storage/entity-framework-core.md",sourceDirName:"storage",slug:"/storage/entity-framework-core",permalink:"/docs/storage/entity-framework-core",draft:!1,unlisted:!1,editUrl:"https://github.com/adimiko/TransactionalBox/tree/main/documentation/docs/storage/entity-framework-core.md",tags:[],version:"current",sidebarPosition:2,frontMatter:{sidebar_position:2},sidebar:"tutorialSidebar",previous:{title:"InMemory",permalink:"/docs/storage/in-memory"},next:{title:"InMemory",permalink:"/docs/transport/in-memory"}},s={},l=[{value:"Install package",id:"install-package",level:2},{value:"Outbox",id:"outbox",level:2},{value:"Register Entity Framework Core as outbox storage",id:"register-entity-framework-core-as-outbox-storage",level:3},{value:"Add Outbox to ModelBuilder",id:"add-outbox-to-modelbuilder",level:3},{value:"Inbox",id:"inbox",level:2},{value:"Register Entity Framework Core as inbox storage",id:"register-entity-framework-core-as-inbox-storage",level:3},{value:"Add Inbox to ModelBuilder",id:"add-inbox-to-modelbuilder",level:3}];function c(e){const n={admonition:"admonition",code:"code",h1:"h1",h2:"h2",h3:"h3",p:"p",pre:"pre",...(0,t.R)(),...e.components};return(0,r.jsxs)(r.Fragment,{children:[(0,r.jsx)(n.h1,{id:"entity-framework-core-relational",children:"Entity Framework Core (Relational)"}),"\n",(0,r.jsxs)(n.p,{children:["TransactionalBox is not responsible for configuring Entity Framework Core.\nAll it needs is to use the already existing ",(0,r.jsx)(n.code,{children:"DbContext"})," and add the model to the ",(0,r.jsx)(n.code,{children:"ModelBuilder"}),"."]}),"\n",(0,r.jsxs)(n.p,{children:["Transactional box add messages to your ",(0,r.jsx)(n.code,{children:"DbContext"}),".\nYou need to invoke ",(0,r.jsx)(n.code,{children:"SaveChangesAsync()"})," on ",(0,r.jsx)(n.code,{children:"DbContext"}),"."]}),"\n",(0,r.jsx)(n.admonition,{type:"warning",children:(0,r.jsxs)(n.p,{children:["Remember, all changes need to be in one transaction.\nUsing ",(0,r.jsx)(n.code,{children:"ExecuteUpdate"})," or ",(0,r.jsx)(n.code,{children:"ExecuteDelete"})," you need to create own transaction."]})}),"\n",(0,r.jsxs)(n.admonition,{type:"tip",children:[(0,r.jsxs)(n.p,{children:["Use ",(0,r.jsx)(n.code,{children:"AddDbContexPoll"})," when configuring Entity Framework Core."]}),(0,r.jsx)(n.pre,{children:(0,r.jsx)(n.code,{className:"language-csharp",children:"builder.Services.AddDbContextPool<SampleDbContext>(x => x.Use...(connectionString));\n"})})]}),"\n",(0,r.jsx)(n.h2,{id:"install-package",children:"Install package"}),"\n",(0,r.jsx)(n.pre,{children:(0,r.jsx)(n.code,{className:"language-csharp",children:"dotnet add package TransactionalBox.EntityFrameworkCore\n"})}),"\n",(0,r.jsx)(n.h2,{id:"outbox",children:"Outbox"}),"\n",(0,r.jsx)(n.h3,{id:"register-entity-framework-core-as-outbox-storage",children:"Register Entity Framework Core as outbox storage"}),"\n",(0,r.jsx)(n.pre,{children:(0,r.jsx)(n.code,{className:"language-csharp",children:"builder.Services.AddTransactionalBox(x =>\n{\n    x.AddOutbox(storage => storage.UseEntityFrameworkCore<SampleDbContext>());\n});\n\n"})}),"\n",(0,r.jsx)(n.h3,{id:"add-outbox-to-modelbuilder",children:"Add Outbox to ModelBuilder"}),"\n",(0,r.jsx)(n.pre,{children:(0,r.jsx)(n.code,{className:"language-csharp",children:"public class SampleDbContext : DbContext\n{\n    ...\n    protected override void OnModelCreating(ModelBuilder modelBuilder)\n    {\n        modelBuilder.AddOutbox();\n    }\n    ...\n}\n"})}),"\n",(0,r.jsx)(n.h2,{id:"inbox",children:"Inbox"}),"\n",(0,r.jsx)(n.h3,{id:"register-entity-framework-core-as-inbox-storage",children:"Register Entity Framework Core as inbox storage"}),"\n",(0,r.jsx)(n.pre,{children:(0,r.jsx)(n.code,{className:"language-csharp",children:"builder.Services.AddTransactionalBox(x =>\n{\n    x.AddInbox(storage => storage.UseEntityFrameworkCore<SampleDbContext>());\n});\n\n"})}),"\n",(0,r.jsx)(n.h3,{id:"add-inbox-to-modelbuilder",children:"Add Inbox to ModelBuilder"}),"\n",(0,r.jsx)(n.pre,{children:(0,r.jsx)(n.code,{className:"language-csharp",children:"public class SampleDbContext : DbContext\n{\n    ...\n    protected override void OnModelCreating(ModelBuilder modelBuilder)\n    {\n        modelBuilder.AddInbox();\n    }\n    ...\n}\n"})})]})}function x(e={}){const{wrapper:n}={...(0,t.R)(),...e.components};return n?(0,r.jsx)(n,{...e,children:(0,r.jsx)(c,{...e})}):c(e)}},8453:(e,n,o)=>{o.d(n,{R:()=>a,x:()=>d});var r=o(6540);const t={},i=r.createContext(t);function a(e){const n=r.useContext(i);return r.useMemo((function(){return"function"==typeof e?e(n):{...n,...e}}),[n,e])}function d(e){let n;return n=e.disableParentContext?"function"==typeof e.components?e.components(t):e.components||t:a(e.components),r.createElement(i.Provider,{value:n},e.children)}}}]);