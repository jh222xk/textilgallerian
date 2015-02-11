@authentication
Feature: Add new discount coupon
    In order to add a new discount coupon
    As an administrator
    I want to be able to add a new discount coupon

Scenario: Add new percentage discount
    Given I am on the add new discount page
		And I have selected the "percentage discount" in the discount type field
		And I have entered "30" in the percentage field
		And I have selected "User" in the code type field
		And I have entered "jh222xk@student.lnu.se" in the customer email field
		And I have selected "April 22, 2015" in the Start Date field
		And I have selected "April 30, 2015" in the End Date field
		And I have selected "can not be combined" in the combinable checkbox
     When I press "Create"
     Then the system should present success
		And a discount of type "percentage discount" should exist
		And a "percentage" with value "30" should exist
		And a "start date" with value April 22, 2015" should exist
		And a "end date" with value April 30, 2015" should exist
		And the discount should not be combinable

Scenario: Add new amount discount
	Given I am on the add new discount page 
		And I have selected the "amount discount" in the discount type field
		And I have entered "200" in the amount field
		And I have selected "User" in the code type field
		And I have entered "jh222xk@student.lnu.se" in the customer email field
		And I have selected "April 22, 2015" in the Start Date field
		And I have selected "April 30, 2015" in the End Date field
		And I have selected "can not be combined" in the combinable checkbox
     When I press "Create"
     Then the system should present success
		And a discount of type "amount discount" should exist
		And a "amount" with value "200" should exist
		And a "start date" with value April 22, 2015" should exist
		And a "end date" with value April 30, 2015" should exist
		And the discount should not be combinable

Scenario: Add new percentage discount on purchase over x kr
    Given I am on the add new discount page
		And I have selected the "discount on purchase over x kr" in the discount type field
		And I have entered "30" in the percentage field
		And I have entered "1000" in the "minimal amount" field
		And I have entered "jh222xk@student.lnu.se" in the customer email field
		And I have selected "can not be combined" in the combinable checkbox
     When I press "Create"
     Then the system should present success
		And a discount of type "discount on purchase over x kr" should exist
		And a "minimal amount" with value "1000" should exist
		And the discount should not be combinable

Scenario: Add new take Y pay for X discount
    Given I am on the add new discount page
		And I have selected the "take Y pay for X discount" in the discount type field
		And I have entered 3 in the take field
		And I have entered 2 in the pay field
		And I have selected "can not be combined" in the combinable checkbox
    When I press "Create"
    Then the system should present success
		And a discount of type "take Y pay for X" should exist
		And a "take" with value "3" should exist
		And a "pay" with value "2" should exist
		And the discount should not be combinable

Scenario: Add new buy product X and recieve product Y discount
	Given I am on the add new discount page
		And I have selected the "buy product X and recieve product Y" in the discount type field
		And I have entered "Blue carpet" in the Buy Products field
		And I have entered "Pink curtains" in the Free Products field
		And I have selected "can not be combined" in the combinable checkbox
	When I press "Create"
	The the system should present success
		And a discount of type "buy product X and recieve product Y" should exist
		And a "buy product" with value "blue carpet" should exist
		And a "free product" with value "pink curtains" should exist
		And a "combinable" with value "can not be combined" should exist

