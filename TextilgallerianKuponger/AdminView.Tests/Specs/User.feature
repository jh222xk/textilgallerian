Feature: Add a new User 
	In order to add a new user 
	As an administrator
	I want to be able to add a new User

@admin
Scenario: Add a new user with read permission
	Given I am on the add new user page 
		And I have entered "linus@textilgallerian.se" in the "Email" field
		And I have entered "password" in the "Password" field
		And I have selected "Read" in the "Role" dropdown
	When I press "Skapa användare"
	Then the system should present "Användare sparad!"
		And a user with email "linus@textilgallerian.se" should exist
		And the user "linus@textilgallerian.se" should have the role "Read"

@admin	
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

@editor	
Scenario: Add a new user when not beeing admin
	Given I am on the users page 
	When I visit the the user creation page
	Then the system should present "Du har inte behörighet att visa sidan!"