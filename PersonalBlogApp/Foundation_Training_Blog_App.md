

Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 1



## Foundation Training

## Personal Blog Application

ASP.NET Core MVC  ·  Entity Framework Core 8  ·  SQL Server  ·  Local File Storage



Duration 2 weeks  (10 working days)
Team Size 1 intern + 1 mentor/lead
Difficulty Beginner to Intermediate  — no prior .NET experience required
Deliverable A fully working blog application running on localhost
Prepared by ITC Group  — Engineering Training Team
## Version 1.0   |   21 May 2026

This document is your complete guide for the two-week training sprint. Read each section carefully before starting your tasks. Your
mentor is available for questions; do not stay blocked for more than 30 minutes without reaching out.


Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 2

## 01  Business Context

About ITC Group

ITC Group is a software outsourcing and product development company based in Ho Chi Minh City, Vietnam. We build enterprise-grade
web applications, microservices, and cloud solutions for clients across Southeast Asia, Singapore, and the US. Our engineering teams
work with modern .NET, Java, and cloud-native stacks.
## Why This Project?

As a new engineering intern at ITC, you will be expected to contribute to real client projects that involve ASP.NET Core, Entity Framework
Core, and team-based workflows. This training project is a safe environment that mirrors exactly how we work — feature branches, pull
request reviews, deployment pipelines, and code standards. Completing it gives you the foundation to onboard onto a live project within
your first month.
## What You Are Building

A multi-user blog platform with role-based access control, rich-text content editing, avatar management, and cloud deployment. While
the domain is a blog, the patterns you will practise — authentication, authorization, file storage, CRUD with EF Core, pagination, and unit
testing — are identical to what you will encounter on production projects at ITC.

## 02  Application Features

Before you write a single line of code, read this section carefully. It describes the complete web application you will build — what pages
exist, who can use each feature, and what the expected behaviour is. Keep this as your reference throughout the two weeks.
## User Roles

Role Who is this? What can they do?
## USER
Any registered member.
Assigned by default after
registration.
Register · Log in/out · Create, edit, delete their OWN blogs · Set blog
priority · Comment on any blog · Delete their OWN comments ·
Upload/update profile avatar
## ADMIN
Assigned manually by the lead.
Cannot self-register as ADMIN.
Everything a USER can do, PLUS: view & manage ALL users' blogs · delete
ANY comment · access User Management · change user roles · activate or
deactivate any account

## Feature List

# Feature What it does Who Can Use It
1 User Registration A visitor provides username, email, password, and
confirm password. On submit, the account is created with
the USER role. Passwords are hashed automatically —
never stored in plain text. The user is redirected to the
Login page.
Anyone (not logged in)
2 Login & Logout Login with email and password. The server verifies
credentials and issues an authentication cookie. Logout
clears the cookie and redirects to the Login page. After
login, the user lands on the Blog List.
Anyone (not logged in)

Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 3
# Feature What it does Who Can Use It
3 Blog List The main page after login. Shows a paginated list of blog
posts. A USER sees only their own posts. An ADMIN sees
every post from all users. Each row shows title, author,
priority badge, and created date.
USER (own posts only)  |  ADMIN (all
posts)
4 Create Blog Post A form with three fields: Title (plain text), Content
(TinyMCE rich-text editor — supports bold, headings,
images), and Priority (1 = lowest to 5 = highest). On
submit, the post is saved and linked to the logged-in user.
All logged-in users
5 Edit Blog Post Opens a pre-filled form to update Title, Content, and
Priority of an existing post. Before showing the form, the
server checks that the requester is either the post author
or an ADMIN. Unauthorised access is rejected with a 403
error.
Post author or ADMIN only
6 Delete Blog Post Permanently deletes the post and all its comments
(cascade delete). Ownership is verified server-side before
deletion — a USER cannot delete another user's post
even by crafting a direct URL request.
Post author or ADMIN only
7 Blog Detail Page Shows the full post content rendered from HTML
(TinyMCE output), the author's name and avatar, created
date, and priority badge. Below the post is the full
comments section and an inline Add Comment form.
All logged-in users
8 Priority Sort Adds a sort option on the Blog List: clicking Sort by
Priority reorders the list from highest (5) to lowest (1)
priority. Implemented via URL query param ?sort=priority
and works alongside search and pagination.
All logged-in users
9 Search & Filter A search bar filters posts by title or content (case-
insensitive SQL LIKE). A Priority dropdown filters to a
specific level. Both filters work together and are reflected
in the URL so the view can be bookmarked or shared.
All logged-in users
10 Pagination The Blog List shows 10 posts per page. Navigation
controls (Previous / page numbers / Next) appear at the
bottom. Page number, sort order, and search query are
all kept in the URL query string.
All logged-in users
11 Add Comment From the Blog Detail page, a logged-in user types a
comment and submits. The comment is saved with the
commenter's name and timestamp and immediately
appears in the comments list. Anonymous users cannot
comment.
All logged-in users
12 Delete Comment Each comment shows a Delete button only to: (a) the user
who wrote it, or (b) an ADMIN. The button is hidden for
everyone else. A direct POST request without permission
also returns a 403.
Comment author or ADMIN only
13 Avatar Upload From the Profile page, the user selects an image file (JPG,
PNG, or GIF, max 2 MB). The server validates the file type
and size, resizes it to 200x200 px using
SixLabors.ImageSharp, and saves it to wwwroot/avatars/.
The relative path is stored in User.AvatarUrl. The avatar
then appears in the navbar and next to every post and
comment by that user.
All logged-in users
14 User List (ADMIN) A table listing all registered users. Searchable by
username or email. Columns: name, email, role badge,
ADMIN only

Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 4
# Feature What it does Who Can Use It
and active/inactive status. Accessible only to ADMIN —
any other role is redirected away.
## 15 Edit User Role & Status
## (ADMIN)
Allows an ADMIN to change a user's role (USER <->
ADMIN) and toggle their account status (active / inactive).
An inactive user cannot log in — they see an error on the
Login page.
ADMIN only
16 Delete User (ADMIN) Permanently deletes the user account plus all associated
blogs and comments. A confirmation step is shown before
the delete executes.
ADMIN only

Page Map  —  Every URL in the Finished App

Use this as a delivery checklist. Every row below must work on the deployed URL by Day 10.
URL Page Name What the user sees
## /
Home / Dashboard Welcome message + Login / Register buttons. Redirects logged-
in users to /blogs.
## /auth/register
Register Registration form. On success, redirects to /auth/login.
## /auth/login
Login Login form. On success, sets cookie and redirects to /blogs.
## /auth/logout
Logout (POST) Clears auth cookie. Redirects to /auth/login.
## /blogs
Blog List Paginated list. Search bar, priority filter, sort toggle, Create Post
button.
## /blogs/create
Create Blog Title field, TinyMCE editor, Priority 1–5 dropdown. POST saves
and redirects to list.
## /blogs/{id}
Blog Detail Full content, author avatar, priority badge, comments section,
Add Comment form.
## /blogs/{id}/edit
Edit Blog Pre-filled form. Author or ADMIN only — others are redirected
with 403.
## /profile/avatar
Avatar Upload Current avatar preview + upload form. Accepts JPG/PNG/GIF up
to 2 MB.
## /users
User List ADMIN only. Searchable table of all users with role and status
columns.
## /users/{id}/edit
Edit User ADMIN only. Change role, toggle active/inactive.

## Key User Stories

User stories define what done really means from the user's perspective. Keep these visible as you code each feature.
ID As a... I want to...   so that...
## US-
## 01
Visitor register an account with my email and password,  so that  I can start writing and publishing blog
posts.
## US-
## 02
USER see only my own blog posts on the main list,  so that  I am not confused by or responsible for
other users' content.

Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 5
ID As a... I want to...   so that...
## US-
## 03
USER write a post using a rich-text editor,  so that  I can add headings, bold text, and images to make my
content look professional.
## US-
## 04
USER assign a priority level (1–5) to my post,  so that  my most important posts appear at the top when
sorted by priority.
## US-
## 05
USER edit or delete only my own posts,  so that  I have full control over my content and cannot
accidentally affect anyone else's.
## US-
## 06
USER leave a comment on any blog post,  so that  I can share feedback and engage with other writers
on the platform.
## US-
## 07
USER upload a profile photo,  so that  my avatar appears next to my posts and comments and makes my
profile recognisable.
## US-
## 08
USER search posts by keyword and filter by priority,  so that  I can quickly find the content I am looking
for without scrolling through every page.
## US-
## 09
ADMIN view, edit, and delete any blog post or comment across the platform,  so that  I can moderate
inappropriate or harmful content.
## US-
## 10
ADMIN view all user accounts, change their roles, and deactivate them,  so that  I can control who has
access and what permissions they hold.
## US-
## 11
ADMIN only be assigned the ADMIN role by the development team (not through self-registration),  so that
elevated access cannot be claimed by anyone who signs up.

## 03  Training Goals

By the end of this two-week sprint, you should be able to:
- Build and structure a complete ASP.NET Core MVC application from scratch.
- Design a relational database schema and manage it with EF Core Code-First migrations.
- Implement secure authentication and role-based authorization using ASP.NET Core Identity.
- Perform full CRUD operations with proper ownership checks and input validation.
- Handle file uploads, validate files server-side, resize images, and save them to the local file system.
- Write unit tests using xUnit and Moq following the Arrange-Act-Assert pattern.
- Apply baseline security practices: XSS prevention, CSRF tokens, rate limiting.
- Run and demo a complete, fully working web application locally with SQL Server.
- Work effectively in a team: feature branches, pull requests, code reviews, and daily standups.

Soft-skill goals:
- Communicate blockers early — do not stay stuck silently for more than 30 minutes.
- Write clear PR descriptions that explain what changed and why.
- Give and receive constructive code review feedback professionally.
- Document your work so that a new team member can understand it without asking you.

## 04  Technology Stack


Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 6
Layer Technology Purpose in This Project
Runtime .NET 8 SDK
Application runtime and build toolchain
Web Framework ASP.NET Core MVC
MVC pattern, routing, controllers, Razor views
ORM Entity Framework Core 8
Code-First migrations, LINQ queries, relationships
Database SQL Server 2022
Relational data storage, all entities
Auth ASP.NET Core Identity
User management, password hashing, role claims
Rich Text TinyMCE (CDN)
WYSIWYG editor for blog content
UI Framework Bootstrap 5
Responsive layout, components, utility classes
Dynamic UI jQuery / Fetch API
AJAX comment posting, form interactions
## File Storage Local File System (wwwroot)
Avatar images stored in wwwroot/avatars/
Image SixLabors.ImageSharp
Server-side image resizing before saving locally
Testing xUnit + Moq
Unit testing with mocked dependencies
Cloud Hosting localhost + local SQL Server
Development and demo environment
Version Ctrl Git + GitHub
Source control, branching, pull requests
IDE Visual Studio 2022 / VS Code
Development environment

## 05  Project Architecture Overview

The application follows a standard ASP.NET Core MVC layered architecture:
Presentation Layer Razor Views (.cshtml) + ViewModels — what the user sees and interacts with
Controller Layer Controllers (Auth, Blogs, Comments, Users, Profile) — handle HTTP requests and route logic
Service / Logic Business logic lives in controllers for this training project; refactor to Services in future
Data Layer Entity Framework Core DbContext — maps C# objects to SQL Server tables
Database SQL Server — stores Users, Blogs, Comments with FK constraints
File Storage Local file system (wwwroot/avatars/) — stores avatar images; relative path saved to
User.AvatarUrl
Auth Layer ASP.NET Core Identity + Cookie Authentication — handles all identity concerns

Key design decisions:
- USER role: can only manage their own Blogs and Comments.
- ADMIN role: has full access to all Blogs, Comments, and the User Management area.
- All sensitive operations (edit, delete) perform an ownership check before executing.
- Blog content is stored as sanitised HTML from TinyMCE to support rich formatting.
- Avatars are resized server-side before upload to keep storage costs low.


Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 7
## 06  Reference Documents & Resources

Use these official resources throughout the two weeks. Bookmark them — they will answer most of your questions.
# Document / Resource Link When to Use
1 ASP.NET Core MVC
## Overview
https://learn.microsoft.com/en-us/aspnet/core/mvc/overview Start here — Week
## 1, Day 1
2 EF Core Getting Started
https://learn.microsoft.com/en-us/ef/core/get-
started/overview/first-app
Day 1–2, migrations
3 ASP.NET Core Identity
https://learn.microsoft.com/en-
us/aspnet/core/security/authentication/identity
Day 2, auth setup
4 EF Core Relationships
https://learn.microsoft.com/en-us/ef/core/modeling/relationships Day 1, schema
design
5 Authorization in ASP.NET
## Core
https://learn.microsoft.com/en-
us/aspnet/core/security/authorization/introduction
Day 2, role policies
6 TinyMCE Quick Start
https://www.tiny.cloud/docs/tinymce/latest/cloud-quick-start/ Day 3, rich text
editor
7 SixLabors ImageSharp Docs
https://docs.sixlabors.com/articles/imagesharp/gettingstarted.html Day 6, image
resizing
8 ASP.NET Core File Uploads
https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-
uploads
Day 5, avatar
upload
9 xUnit Documentation
https://xunit.net/docs/getting-started/netcore/cmdline Day 9, unit testing
10 X.PagedList for ASP.NET
## Core
https://github.com/dncuug/X.PagedList Day 8, pagination
## 11 Bootstrap 5 Documentation
https://getbootstrap.com/docs/5.3/getting-started/introduction/ UI throughout
12 OWASP Top 10
https://owasp.org/www-project-top-ten/ Day 9, security
review
13 xUnit + Moq Testing Guide
https://learn.microsoft.com/en-us/dotnet/core/testing/unit-
testing-with-dotnet-test
Day 8, unit tests
## 14 Git Branching Model
https://nvie.com/posts/a-successful-git-branching-model/ Entire sprint — Git
workflow

## 07  Working Agreements

Please follow these conventions throughout the sprint. They are not optional — they reflect how real ITC teams operate.
## Git & Code
- Branch naming: feature/task-id  (e.g. feature/t3-1-blog-create)
- Commit messages: short, imperative — Add blog create action, not Fixed stuff
- Never commit directly to main; always work on a feature branch and open a PR
- PR must be reviewed by at least one other intern before the lead reviews it

## Communication

Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 8
- Daily standup format: What did I do yesterday? What will I do today? Any blockers?
- If blocked for more than 30 minutes, ask for help — do not struggle silently
- Write all Slack messages, PR descriptions, and code comments in English

## Code Standards
- Use PascalCase for class and method names; camelCase for local variables
- Never hardcode connection strings, API keys, or secrets — use appsettings.json or environment variables
- Every controller action that modifies data must include an ownership or role check
- Remove all TODO comments and unused code before raising a PR

08  2-Week Task Schedule

Read each row carefully. The Expected Outcome column is your acceptance criteria — it defines what 'done' means for each task.
Outcome legend:   Task done — deliverable  |  Tech — must be able to explain  |  Team — collaboration requirement
## Day Task Description Expected Outcome Next Steps Assignee
Week 1  —  Foundation   —   Environment · Auth · Blog CRUD · Comments · User Management · Avatar Upload
## Day 1
## T 1.1
## Dev Environment Setup

Install .NET 8 SDK, Visual Studio 2022,
SQL Server 2022, SSMS, and Git.
Create a new ASP.NET Core MVC project
and verify it starts at localhost:5000.
Task done:
- App runs at localhost:5000 without errors
- Git repo initialised and first commit pushed
- SQL Server connection confirmed in SSMS

## Tech:
- Understand ASP.NET Core MVC folder
structure
- Understand .csproj and launchSettings.json
roles

## Team:
- Report setup result to lead via Slack/Teams
→ Read Microsoft ASP.NET Core
MVC overview docs
→ Study Model-View-Controller
pattern
→ Prepare entity model design for
tomorrow
## Solo
## T 1.2
## Database Schema Design

Design 3 entity classes: User, Blog,
Comment with all required fields and
relationships.
Draw an ERD diagram using
dbdiagram.io or draw.io and get it
approved by the lead.
Task done:
- ERD diagram complete with all FK
relationships
- 3 entity classes coded with correct data
types
- Lead has reviewed and approved the
schema

## Tech:
- Understand 1-to-many FK relationships in
EF Core
- Understand Data Annotations vs. Fluent
## API

## Team:
- ERD shared in team channel before writing
any code
→ Study EF Core Code-First
migrations
→ Prepare DbContext and seed data
## Solo
T 1.3 EF Core Migrations & Seed Data

Task done:
- Migration runs without errors
- Schema matches ERD (verified in SSMS)
→ Study ASP.NET Core Identity
integration
→ Prepare AuthController structure
## Solo

Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 9
## Day Task Description Expected Outcome Next Steps Assignee
Create DbContext, configure entity
relationships, run the initial migration.
Seed 1 admin user + 3 sample blogs into
the local SQL Server database.
- Seed data visible and queryable

## Tech:
- Understand Add-Migration and Update-
Database commands
- Understand OnModelCreating() and its
purpose

## Team:
- Commit connection string pattern to
README (no passwords)
## Day 2
## T 2.1
ASP.NET Core Identity & Auth

Integrate Identity into the project.
Configure cookie authentication.
Build Register and Login pages (Views +
Controller + validation).
Task done:
- Register creates user with USER role by
default
- Login sets auth cookie and redirects to
blog list
- Logout clears session; form validation
works

## Tech:
- Understand IdentityUser and
PasswordHasher internals
- Understand [Authorize] and claims-based
identity

## Team:
- Demo full auth flow to lead before end of
day
→ Read about JWT vs. Cookie auth
trade-offs
→ Prepare role-based authorization
## (T2.2)
## Solo
T 2.2 Role-Based Authorization

Configure USER and ADMIN
authorization policies.
Apply [Authorize(Roles=...)] to all
relevant controllers. Test all access-
control paths.
Task done:
- USER is blocked from /users (ADMIN-only
area)
- ADMIN can access all routes
- Unauthenticated users are redirected to
## Login

## Tech:
- Understand Authentication vs.
Authorization clearly
- Understand IAuthorizationPolicyProvider

## Team:
- Write a test-case list and send to lead for
review
## → Study Claims Transformation
pattern
→ Prepare Blog CRUD (T3.1)
## Solo
## Day 3
T 3.1 Blog CRUD — Create & List

BlogsController Index (USER sees own
blogs; ADMIN sees all).
Create action GET + POST. Integrate
TinyMCE rich-text editor for Content
field.
Task done:
- USER sees only their own blogs
- ADMIN sees all blogs across all users
- TinyMCE loads and saves HTML content
correctly

## Tech:
- Understand ViewModel pattern
(BlogCreateVM)
- Understand View <-> Controller data flow

## Team:
→ Study XSS prevention when
rendering HTML content
→ Prepare Edit, Delete, Details (T3.2)
## Solo

Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 10
## Day Task Description Expected Outcome Next Steps Assignee
- Raise PR with clear description; lead
reviews
T 3.2 Blog CRUD — Edit, Delete, Details &
## Priority Sort

Edit/Delete with ownership check.
Details page layout.
Priority field (1–5 dropdown). Sort
query: /blogs?sort=priority.
Task done:
- Edit/Delete restricted to post owner or
## ADMIN
- Priority sort works correctly (highest first)
- Priority displayed as a colour-coded badge

## Tech:
- Understand IQueryable chaining in EF Core
- Understand TempData vs. ViewBag vs.
ViewData

## Team:
- Walk the lead through the sort
implementation
→ Research X.PagedList for
pagination (Week 2)
→ Prepare Comments feature (T4.1)
## Solo
## Day 4
## T 4.1
## Comments — Create & Display

CommentsController Create (POST,
authenticated only).
Render a comments section inside Blog
Detail view via a Partial View.
Task done:
- Comment POST requires login; anonymous
users are blocked
- Comments display correctly below the blog
content
- Commenter name and timestamp are
shown

## Tech:
- Understand Include() / ThenInclude() for
eager loading
- Understand Partial Views for reusable UI

## Team:
- Self-review code before raising PR
→ Study AJAX POST with jQuery /
Fetch API
→ Prepare Delete comment +
optional AJAX (T4.2)
## Solo
T 4.2 Comments — Delete & AJAX (optional)

Delete comment (owner or ADMIN only).
Optional stretch: AJAX-based comment
posting — no full page reload.
Task done:
- Delete button visible only to owner or
## ADMIN
- Unauthorised delete returns 403 gracefully
- [Optional] New comment appears without
page reload

## Tech:
- Understand AntiForgeryToken with AJAX
requests
- Understand JsonResult vs. IActionResult

## Team:
- Document the AJAX pattern in code
comments
→ Study SignalR for real-time
comments (future)
→ Prepare User Management (T5.1)
## Solo
## Day 5
T 5.1 User Management (ADMIN)

UsersController: Index with search/filter
by name or email,
Edit to update role or toggle
active/inactive, Delete user.
Task done:
- ADMIN sees full user list; search by
name/email works
- Role change persists correctly to DB
- Inactive users are blocked from logging in

## Tech:
→ Study soft delete vs. hard delete
patterns
→ Prepare Avatar Upload (T5.2)
## Solo

Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 11
## Day Task Description Expected Outcome Next Steps Assignee
- Understand UserManager<T> from
## Identity
- Understand querying Identity users with EF
## Core

## Team:
- Demo user management to lead before
end of day
## T 5.2 Avatar Upload — Local File Storage

ProfileController: GET form + POST
handler.
Validate file type (jpg/png/gif) and max 2
## MB.
Save to wwwroot/avatars/. Update
User.AvatarUrl in DB.
Task done:
- Upload succeeds; avatar renders in profile
and navbar
- Invalid file type or oversized file is rejected
with message
- AvatarUrl saved correctly in DB

## Tech:
- Understand IFormFile, Path.Combine,
wwwroot structure
- Understand server-side validation with
ModelState

## Team:
- Demo avatar upload flow to lead
→ Study SixLabors.ImageSharp for
image resizing (T6.2)
→ Pre-read pagination docs before
## Week 2
## Solo
## T 5.3
## Week 1 Integration & Code Review

Merge all feature branches to main.
Resolve conflicts.
Lead reviews all code. Solo retrospective:
what went well, what to improve.
Task done:
- Main branch builds cleanly with no
runtime errors
- All Week 1 features work end-to-end

## Tech:
- Understand Git branching (feature branch
model)
- Know how to resolve merge conflicts
cleanly

## Team:
- Present all features to lead (10 min
walkthrough)
- Receive written feedback and
acknowledge each point
→ Log all bugs from code review as
GitHub Issues
→ Plan Day 6–10 tasks with lead
## Solo + Lead
Week 2  —  Advanced   —   Image Resize · Pagination · Search · Unit Tests · Security · Documentation
## Day 6
T 6.1 Bug Fixes from Week 1 Review

Resolve all issues logged in GitHub Issues
from the code review.
Fix naming violations, remove hardcoded
values, apply lead feedback.
Task done:
- All critical bugs fixed and re-tested
- Code follows agreed naming conventions
- No passwords or secrets anywhere in the
codebase

## Tech:
- Understand common MVC pitfalls: N+1
query, null ViewBag
- Improve debugging skills in VS / VS Code

## Team:
- Update each GitHub Issue status as fixed
→ Study ASP.NET Core middleware
pipeline
→ Prepare image resizing (T6.2)
## Solo

Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 12
## Day Task Description Expected Outcome Next Steps Assignee
T 6.2 Image Resizing with
SixLabors.ImageSharp

Install SixLabors.ImageSharp NuGet
package.
Before saving an uploaded avatar, auto-
resize to 200×200 px while maintaining
aspect ratio.
Task done:
- All uploaded avatars auto-resized to
200×200 px
- Output file size consistently under 50 KB
- Image is not distorted or stretched

## Tech:
- Understand stream-based file processing
in .NET
- Understand ImageSharp Mutate API basics

## Team:
- Raise PR; lead reviews before merging
→ Study WebP format for better
performance (future)
→ Prepare Blog Pagination (T7.1)
## Solo
## Day 7
## T 7.1
## Blog List Pagination

Add server-side pagination to
BlogsController.Index
using X.PagedList.Mvc.Core NuGet
package. Default: 10 items per page.
Task done:
- 10 blogs per page; page controls render
correctly
- Sorting and pagination work together
without conflict
- Current page is preserved when navigating
back

## Tech:
- Understand Skip/Take in LINQ queries
- Understand IPagedList<T> interface

## Team:
- Explain pagination approach to lead in
standup
→ Study infinite scroll as alternative
UX (future)
→ Prepare Search & Filter (T7.2)
## Solo
## T 7.2 Blog Search & Filter

Search bar for title and content (case-
insensitive SQL LIKE).
Filter by priority. Combine with sort and
pagination via URL params:
## ?search=...&priority=...&sort=...&page=...
Task done:
- Search returns correct, case-insensitive
results
- Priority filter works independently and
combined
- URL is bookmarkable; state survives page
refresh

## Tech:
- Understand EF.Functions.Like for LINQ text
search
- Understand [FromQuery] parameter
binding

## Team:
- Demo to lead; feedback logged in GitHub
## Issues
→ Study SQL Server Full-Text Search
for larger datasets
→ Prepare unit tests (T8.1)
## Solo
## Day 8
## T 8.1 Unit Tests — Auth & Blog Logic

Write xUnit tests for Register/Login logic
(mock UserManager).
Test blog filtering (USER vs. ADMIN
view). Target: at least 8 passing test
cases.
Task done:
- At least 8 test cases pass
- Coverage includes happy path and key
error cases
- Tests follow Arrange-Act-Assert naming
and structure

## Tech:
- Understand Arrange-Act-Assert pattern
→ Study integration tests with
WebApplicationFactory
→ Study Coverlet for code coverage
reporting
## Solo

Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 13
## Day Task Description Expected Outcome Next Steps Assignee
- Understand Moq for mocking
dependencies

## Team:
- Lead reviews test quality — not just
quantity
## T 8.2 Security Hardening

Review and fix: XSS prevention
(@Html.Raw usage audited), CSRF
tokens on all forms,
HTTPS redirect enforced, input validation
tightened,
basic rate limiting on Login endpoint
(max 5 attempts).
Task done:
- No XSS vulnerabilities in blog or comment
content
- AntiForgeryToken present on every POST
form
- Login rate-limited to 5 attempts

## Tech:
- Understand OWASP Top 10 fundamentals
## • Understand Content Security Policy
headers

## Team:
- Deliver a short security checklist to lead
→ Study OWASP ZAP for automated
security testing
→ Study ASP.NET Core Data
Protection API
## Solo
## Day 9
## T 9.1
UI Polish — Layout & Navigation

Update _Layout.cshtml: responsive
Bootstrap 5 navbar,
breadcrumb navigation, _LoginPartial,
avatar in navbar when logged in.
Task done:
- Navbar is responsive on mobile, tablet, and
desktop
- Avatar renders correctly for the logged-in
user
- Active nav item is highlighted

## Tech:
- Understand Razor tag helpers: asp-route,
asp-action, asp-controller
- Understand Bootstrap 5 grid and utility
classes

## Team:
- Get UX feedback from lead before merging
→ Study CSS custom properties for
theming
→ Prepare documentation (T9.2)
## Solo
## T 9.2
Documentation & README

Write a complete README.md: project
overview, local setup guide,
feature list, known issues. Include an
architecture diagram in /docs.
Task done:
- A new developer can set up the project
locally in 15 minutes
- Architecture diagram committed in /docs/
- No magic numbers or hardcoded values
remain

## Tech:
- Write clear, structured technical
documentation
- Understand its value for onboarding future
developers

## Team:
- Lead reviews README and gives final sign-
off
→ Study ADR (Architecture Decision
## Records)
→ Learn Swagger / OpenAPI for API
documentation
## Solo
## Day 10
## T 10.1
## Final Demo & Retrospective

Task done:
- All 16 features demonstrated live on
localhost
→ Next topic: build a REST API with
ASP.NET Core Web API
## Solo + Lead

Foundation Training  —  ITC Group
## Personal Blog Application

ITC Group  ·  Confidential — For Intern Use Only     Page 14
## Day Task Description Expected Outcome Next Steps Assignee
Present the completed application to the
lead — live walkthrough of all 16
features.
Technical Q&A session. Submit a 1-page
self-assessment.
- Technical decisions explained clearly and
confidently

## Tech:
- Able to explain the full architecture end-to-
end
- Can answer technical questions without
referring to notes

## Team:
- 1-page self-assessment submitted to lead
- Receive and acknowledge written
feedback
→ Learn Docker and containerisation
fundamentals
→ Prepare to contribute to a live ITC
project

## 09  Success Criteria

At the end of the sprint, your lead will evaluate you on three dimensions:
Dimension What We Look For How to Demonstrate It
## Technical Delivery
All assigned tasks completed and working
on the deployed URL
Live demo on Day 10; app works end-to-end
without critical bugs
## Technical Understanding
Can explain how each feature works, not
just that it works
Answer lead's technical questions in the Day 10
Q&A session
## Team Collaboration
Communicates proactively, reviews peers'
code, asks for help early
PR quality, standup participation, peer feedback
quality on Day 10

Questions? Reach out to your assigned mentor on Slack. Good luck — we are rooting for you.