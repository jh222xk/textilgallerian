@ignore
Feature: Login
	In order to login
	As a currently logged out user
	I want to be logged in 

Scenario: Navigate to the page
    When I navigate to /
    Then I would need to login
    Then I should be able to add a new discount
