@ignore
@admin
Feature: Add a new User 
	In order to add a new user 
	As an administrator
	I want to be able to add a new User

Scenario: Add a new user with read permission
	Given I am on the users page 
	When I press "Lägg till ny användare"
		And I enter "Lucas@Textilgallerian.se" in the "Email" field
		And I have entered "password" in the "Password" field
		And I have entered "password" in the "PasswordConfirmation" field
		And I have selected "läsa" in the "Role" dropdown
	Then the system should present "Användaren har skapats"
		And a user with email "Lucas@Textilgallerian.se" and password "password" should exist
		And the user "lucas@textilgallerian.se" should have the role "läsa"
		
Scenario: Add a new user with update permission
	Given I am on the users page 
	When I press "Lägg till ny användare"
		And I enter "Per@Textilgallerian.se" in the "Email" field
		And I have entered "secure" in the "Password" field
		And I have entered "secure" in the "PasswordConfirmation" field
		And I have selected "redigera" in the "Role" dropdown
	Then the system should present "Användaren har skapats"
		And a user with email "Per@Textilgallerian.se" and password "secure" should exist
		And the user "per@textilgallerian.se" should have the role "redigera"
		
Scenario: Add a new user with invalid password confirmation
	Given I am on the users page 
	When I press "Lägg till ny användare"
		And I enter "Emil@Textilgallerian.se" in the "Email" field
		And I have entered "password" in the "Password" field
		And I have entered "secure" in the "PasswordConfirmation" field
		And I have selected "admin" in the "Role" dropdown
	Then the system should present "Lösenordsbekräftelsen stämmer inte"
		And no user with email "Emil@Textilgallerian.se" should exist
		
Scenario: Add a new user when not beeing admin
	Given I am signed in as an user with role "redigera"
		And I am on the users page 
	When I am on the users page 
	Then the link "Lägg till ny användare" should be missing