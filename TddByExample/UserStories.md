# User Stories

## Story Title: Changing a username

---

### Story Description

**As a customer:**

Given I already have an account

When I navigate to my profile page

Then I can update my username

### Acceptance Criteria

The username can only contain between 8 and 12 characters, inclusive:

- Valid: `AnameOf8`, `NameOfChar12`
- Invalid: `AnameOfChar13`, `NameOf7`

Only alphanumerics and underscores are allowed:

- Valid: `Letter_123`
- Invalid: `!The_Start`, `IneThe@Middle`, `WithDollar$`, `Space 123`

If the username already exists, generate an error
