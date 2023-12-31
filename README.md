﻿<h1 align="center">Hi 👋, I'm Goldin Baloyi</h1>
<h3 align="center">(A passionate full-stack developer from Sandton [SA])</h3>
<p>This is a project consits of an API which uses JWT Auth to create customers accounts, and manages the debits and credits transactions associated with those accounts.</P>
<h4>Getting started.</h4>
<ul>
<li>This is a C# .net 7 application. <b>Please make sure the correct SDK is installed before running the application</b>.</li>
 <li>Log activity can be located on console while application is running and also on the windows event.</li>
  <li>The application uses in-memory MS SQL database as a main storage, the auth db is a MS SQL (localdb) and also Redis as cache data storage needed by the application.</li>
  <li>Sample files can be found on the <b>'Samples'</b> folder</>
  <li>Application technical documentation can be found on the <b>'VSdoc'</b> folder, just open the index.html page.</>
  <li>Application functional specification can be found on the document called <b>'Goldin Account Manager Functional Spec.docx'</b>.</>
</ul>
<h4>How it all works.</h4>
<ol>
<li>You first need to authenticate your API user to get an <b>access token</b>, if no user available, you can create a new one. <u>Note</u> that the token expires, if that happens, please <b>re-authenticate</b> to get a new access token.</li>
<li>After you have logged in, you will <b>need to create accounts</b>, if you only have one account, data will be loaded straight from the database else if will be loaded from a Redis cached database on the 2nd load.</li>
<li><b>You can not use the credit or debit functionality if no accounts exists</b>, make use of sample data if required.</li>
<li>Once you have accounts, you can now perform debits and credits to account(s).</li>
<li><b>No debits</b> can be performed on accounts with <b>zero balances</b> and if the debit amount is greater than the available balance, the debit request will be rejected.</li>
<li>Statements for account can be pulled on a daterange and also shows the balance for that perticular period, transactions are pulled similarly to how accounts are pulled, either on database or on the cached database.</li>
<ol>
<br/>
<h4>Preview of application.</h4>
<p>Main application.</p>

![](https://github.com/Goldin123/images/assets/17449653/c275f198-1c04-48c7-99b8-5a347f5cc364)

<p>Console application.</p>

![](https://github.com/Goldin123/images/assets/17449653/0e8857e6-b1ed-4080-9383-25e46ef78afe)
