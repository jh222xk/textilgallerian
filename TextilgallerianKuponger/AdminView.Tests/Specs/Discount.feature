@ignore
@authentication
Feature: Add new discount coupon
    In order to add a new discount coupon
    As a administrator
    I want to be able to add a new discount coupon

Scenario: Add new percentage discount
    Given I am on the add new discount page
		And I have selected the "percentage discount" in the discount type field
		And I have entered "30" in the percentage field
		And I have entered "jh222xk@student.lnu.se" in the customer email field
     When I press "Create"
     Then the system should present success
		And a discount of type "percentage discount" should exist
		And a "percentage" with value "30" should exist

Scenario: Add new discount on purchase over x kr
    Given I am on the add new discount page
		And I have selected the "discount on purchase over x kr" in the discount type field
		And I have entered "30" in the percentage field
		And I have entered "1000" in the "minimal amount" field
		And I have entered "jh222xk@student.lnu.se" in the customer email field
     When I press "Create"
     Then the system should present success
		And a discount of type "discount on purchase over x kr" should exist
		And a "minimal amount" with value "1000" should exist

Scenario: Add new take Y pay for X discount
    Given I am on the add new discount page
		And I have selected the "take Y pay for X discount" in the discount type field
		And I have entered 3 in the take field
		And I have entered 2 in the pay field
    When I press "Create"
    Then the system should present sucess
		And a discount of type "take Y pay for X" should exist
		And a "take" with value "3" should exist
		And a "pay" with value "2" should exist