@authentication
Feature: Add a new User 
	In order to add a new user 
	As an administrator
	I want to be able to add a new User

Scenario: Add a new User with Read Permission 
	Given I am on the User Page 
		And I have clicked Add new User
		And I have entered "Lucas" in the Name Field
		And I have entered "Lucas@Textilgallerian.se" in the Email Field
		And I have entered "password" in the Password Field
		And I have entered "password" in the Password confirmation Field
		And I have selected "Read" in the Permission Field
	When I press "Create"
	Then the system should present success
		And a new User named "Lucas" should exist
		And the Email of the new User should be "Lucas@Textilgallerian.se"
		And the Permission of the new User should be "Read"
		
Scenario: Add a new User with "Update" Permission 
	Given I am on the User Page 
		And I have clicked Add new User
		And I have entered "Per" in the Name Field
		And I have entered "Per@Textilgallerian.se" in the Email Field
		And I have entered "password" in the Password Field
		And I have entered "password" in the Password confirmation Field
		And I have selected "Update" in the Permission Field
	When I press "Create"
	Then the system should present success
		And a new User named "Per" should exist
		And the Email of the new User should be "Per@Textilgallerian.se"
		And the Permission of the new User should be "Update"
