@logout
Feature: Login
	In order to login
	As a currently logged out user
	I want to be logged in 

Scenario: Navigate to the page
	When I navigate to /
	Then I would need to login

Scenario: Log in
	Given I am on the login page
	When I enter "admin@admin.com" in the "Email" field
		And I enter "password" in the "Password" field
		And I press "Logga in"
	Then I should be logged in

Scenario: Log in with invalid credentials
	Given I am on the login page
	When I enter "linus@textilgallerian.se" in the "Email" field
		And I enter "invalid" in the "Password" field
		And I press "Logga in"
	Then I shouldn't be logged in
		And the system should present "Felaktig epost och/eller lösenord."