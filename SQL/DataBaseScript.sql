-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';
-- -----------------------------------------------------
-- Schema market
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema market
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `market` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `market` ;

-- -----------------------------------------------------
-- Table `market`.`aboutapp`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `market`.`aboutapp` (
  `idAboutApp` INT NOT NULL AUTO_INCREMENT,
  `VersionNum` DOUBLE NOT NULL,
  `VersionName` VARCHAR(45) NOT NULL,
  `Reason For It` VARCHAR(200) NULL DEFAULT NULL,
  PRIMARY KEY (`idAboutApp`),
  UNIQUE INDEX `VersionNum_UNIQUE` (`VersionNum` ASC) VISIBLE,
  UNIQUE INDEX `VersionName_UNIQUE` (`VersionName` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `market`.`companies`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `market`.`companies` (
  `idCompanies` INT NOT NULL AUTO_INCREMENT,
  `CompanyName` VARCHAR(45) NOT NULL,
  `PhoneNumber` DOUBLE NOT NULL,
  `Email` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`idCompanies`),
  UNIQUE INDEX `idCompanies_UNIQUE` (`idCompanies` ASC) VISIBLE,
  UNIQUE INDEX `CompanyName_UNIQUE` (`CompanyName` ASC) VISIBLE,
  UNIQUE INDEX `PhoneNumber_UNIQUE` (`PhoneNumber` ASC) VISIBLE,
  UNIQUE INDEX `Email_UNIQUE` (`Email` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `market`.`orders`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `market`.`orders` (
  `idOrders` INT NOT NULL AUTO_INCREMENT,
  `TotalPrice` DOUBLE NOT NULL,
  `ProfitFromOrder` DOUBLE NOT NULL,
  `OrderDate` DATETIME NOT NULL,
  PRIMARY KEY (`idOrders`),
  UNIQUE INDEX `idOrders_UNIQUE` (`idOrders` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 29
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `market`.`producttypes`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `market`.`producttypes` (
  `idProductTypes` INT NOT NULL AUTO_INCREMENT,
  `ProductTypesName` VARCHAR(45) NOT NULL,
  `TypeProfit` DOUBLE NULL DEFAULT '0',
  PRIMARY KEY (`idProductTypes`),
  UNIQUE INDEX `idProductTypes_UNIQUE` (`idProductTypes` ASC) VISIBLE,
  UNIQUE INDEX `ProductTypesName_UNIQUE` (`ProductTypesName` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `market`.`products`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `market`.`products` (
  `idProducts` INT NOT NULL AUTO_INCREMENT,
  `ProductName` VARCHAR(45) NOT NULL,
  `ProductsTypeID` INT NOT NULL,
  `CompanyID` INT NOT NULL,
  `Quantity` INT NOT NULL,
  `ProductsOGprice` DOUBLE NOT NULL,
  `ProductProfitPrice` DOUBLE NOT NULL,
  `ExpDate` DATE NOT NULL,
  PRIMARY KEY (`idProducts`),
  UNIQUE INDEX `idProducts_UNIQUE` (`idProducts` ASC) VISIBLE,
  INDEX `CompanyOfProduct_idx` (`CompanyID` ASC) VISIBLE,
  INDEX `ProductType_idx` (`ProductsTypeID` ASC) VISIBLE,
  CONSTRAINT `CompanyOfProduct`
    FOREIGN KEY (`CompanyID`)
    REFERENCES `market`.`companies` (`idCompanies`),
  CONSTRAINT `ProductType`
    FOREIGN KEY (`ProductsTypeID`)
    REFERENCES `market`.`producttypes` (`idProductTypes`))
ENGINE = InnoDB
AUTO_INCREMENT = 23
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `market`.`orderproducts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `market`.`orderproducts` (
  `ProductID` INT NOT NULL,
  `OrderID` INT NOT NULL,
  `ProfitPrice` DOUBLE NOT NULL,
  `OGprice` DOUBLE NOT NULL,
  `Quantity` INT NOT NULL,
  PRIMARY KEY (`OrderID`, `ProductID`),
  INDEX `ProductKey_idx` (`ProductID` ASC) VISIBLE,
  CONSTRAINT `OrderKey`
    FOREIGN KEY (`OrderID`)
    REFERENCES `market`.`orders` (`idOrders`),
  CONSTRAINT `ProductKey`
    FOREIGN KEY (`ProductID`)
    REFERENCES `market`.`products` (`idProducts`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
