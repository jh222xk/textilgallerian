@editor
Feature: Add new discount coupon
    In order to add a new discount coupon
    As an editor
    I want to be able to add a new discount coupon

Scenario: Add new percentage discount
    Given I am on the add new discount page
		And I have selected the "TotalSumPercentageDiscount" in the discount type field
		And I have entered "Holiday Season" in the "Name" field
		And I have entered "XMAS15" in the "Code" field
		And I have entered "900105-3001" in the "CustomerString" field
		And I have entered "Test coupon" in the "Description" field
		And I have entered "2015-01-15" in the "Start" field
		And I have entered "2016-04-30" in the "End" field
		And I have entered "2" in the "UseLimit" field
		And I have entered "30" in the "Percentage" field
		And I have checked the coupon can be combined checkbox
     When I press "Skapa rabatt"
     Then the system should present "Rabatt sparad!"
		And a discount of type "Köp för X:kr få Y:% rabatt" should exist
		And it should have a "Kampanjnamn" of "Holiday Season"
		And it should have a "Kampanjkod" of "XMAS15"
		And it should have a "Startdatum" of "2015-01-15"
		And it should have a "Slutdatum" of "2016-04-30"
		And the Holiday Season API test should pass

Scenario: Add new amount discount
    Given I am on the add new discount page
		And I have selected the "TotalSumAmountDiscount" in the discount type field
		And I have entered "Easter Season" in the "Name" field
		And I have entered "Chicken" in the "Code" field
		And I have entered "500" in the "MinPurchase" field
		And I have entered "TotalSumAmountDiscount" in the "Description" field
		And I have entered "2015-01-15" in the "Start" field
		And I have entered "100" in the "Amount" field
     When I press "Skapa rabatt"
     Then the system should present "Rabatt sparad!"
		And a discount of type "Köp för X:kr få Y:kr rabatt" should exist
		And it should have a "Kampanjnamn" of "Easter Season"
		And it should have a "Kampanjkod" of "Chicken"
		And it should have a "Startdatum" of "2015-01-15"
		And the Easter Season API test should pass

Scenario: Add new percentage discount on purchase over x kr
    Given I am on the add new discount page
		And I have selected the "TotalSumPercentageDiscount" in the discount type field
		And I have entered "Holiday Season" in the "Name" field
		And I have entered "XMAS15" in the "Code" field
		And I have entered "900105-3001" in the "CustomerString" field
		And I have entered "Test coupon" in the "Description" field
		And I have entered "2015-04-15" in the "Start" field
		And I have entered "2015-04-30" in the "End" field
		And I have entered "2" in the "UseLimit" field
		And I have entered "30" in the "Percentage" field
		And I have entered "500" in the "MinPurchase" field
		And I have checked the coupon can be combined checkbox
     When I press "Skapa rabatt"
     Then the system should present "Rabatt sparad!"
		And a discount of type "Köp för X:kr få Y:% rabatt" should exist
		And it should have a "Kampanjnamn" of "Holiday Season"
		And it should have a "Kampanjkod" of "XMAS15"
		And it should have a "Startdatum" of "2015-04-15"
		And it should have a "Slutdatum" of "2015-04-30"
		And the Holiday Season API test should pass

Scenario: Add new take Y pay for X discount
    Given I am on the add new discount page
		And I have selected the "BuyXProductsPayForYProducts" in the discount type field
		And I have entered "Summer" in the "Name" field
		And I have entered "Beach" in the "Code" field
		And I have entered "900105-3001" in the "CustomerString" field
		And I have entered "Test coupon" in the "Description" field
		And I have entered "2015-06-01" in the "Start" field
		And I have entered "2015-08-30" in the "End" field
		And I have entered "2" in the "UseLimit" field
		And I have entered "3" in the "Buy" field
		And I have entered "2" in the "PayFor" field
     When I press "Skapa rabatt"
     Then the system should present "Rabatt sparad!"
		And a discount of type "Tag X betala för Y" should exist
		And it should have a "Kampanjnamn" of "Summer"
		And it should have a "Kampanjkod" of "Beach"
		And it should have a "Startdatum" of "2015-06-01"
		And it should have a "Slutdatum" of "2015-08-30"
		And the Summer API test should pass

Scenario: Add new buy product X and recieve product Y discount
    Given I am on the add new discount page
		And I have selected the "BuyProductXRecieveProductY" in the discount type field
		And I have entered "Halloween" in the "Name" field
		And I have entered "pumpkin" in the "Code" field
		And I have entered "100" in the "MinPurchase" field
		And I have entered "Test coupon" in the "Description" field
		And I have entered "900105-3001" in the "CustomerString" field
		And I have entered "2015-09-01" in the "Start" field
		And I have entered "2015-10-30" in the "End" field
		And I have entered "2" in the "UseLimit" field
		And I have entered "3" in the "Buy" field
		And I have entered "3" in the "AmountOfProducts" field
		And I have entered "Pink Curtain" in the "FreeProduct" field
     When I press "Skapa rabatt"
     Then the system should present "Rabatt sparad!"
		And a discount of type "Köp X få Y gratis" should exist
		And it should have a "Kampanjnamn" of "Halloween"
		And it should have a "Kampanjkod" of "pumpkin"
		And it should have a "Startdatum" of "2015-09-01"
		And it should have a "Slutdatum" of "2015-10-30"
		And the Halloween API test should pass