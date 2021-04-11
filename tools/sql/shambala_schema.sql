-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema shambala
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema shambala
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `shambala` DEFAULT CHARACTER SET utf8mb4 ;
USE `shambala` ;

-- -----------------------------------------------------
-- Table `shambala`.`product`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`product` (
  `Name` VARCHAR(20) NOT NULL,
  `Id` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `shambala`.`caret_detail`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`caret_detail` (
  `Id` TINYINT(3) UNSIGNED NOT NULL,
  `CaretSize` TINYINT(4) NOT NULL,
  `GSTRate` TINYINT(4) NOT NULL,
  `CaretPrice` DECIMAL(6,2) NOT NULL,
  `Product_Id_FK` INT(10) UNSIGNED NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `Product_Caret_Relationship_idx` (`Product_Id_FK` ASC) VISIBLE,
  CONSTRAINT `Product_Caret_Relationship`
    FOREIGN KEY (`Product_Id_FK`)
    REFERENCES `shambala`.`product` (`Id`)
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `shambala`.`flavour`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`flavour` (
  `Id` TINYINT(4) UNSIGNED NOT NULL AUTO_INCREMENT,
  `Title` VARCHAR(20) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `shambala`.`incoming_shipment`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`incoming_shipment` (
  `Id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `Product_Id_FK` INT(11) UNSIGNED NOT NULL,
  `TotalRecievedPieces` SMALLINT(5) UNSIGNED NULL DEFAULT NULL,
  `TotalDefectPieces` SMALLINT(5) UNSIGNED NULL DEFAULT NULL,
  `CaretSize` TINYINT(4) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE,
  INDEX `Incoming_shipment_Product_Relationship_idx` (`Product_Id_FK` ASC) VISIBLE,
  CONSTRAINT `Incoming_shipment_Product_Relationship`
    FOREIGN KEY (`Product_Id_FK`)
    REFERENCES `shambala`.`product` (`Id`)
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `shambala`.`salesman`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`salesman` (
  `Id` SMALLINT(5) UNSIGNED NOT NULL AUTO_INCREMENT,
  `FullName` VARCHAR(60) NOT NULL,
  `IsActive` TINYINT(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `shambala`.`outgoing_shipment`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`outgoing_shipment` (
  `Id` INT(11) UNSIGNED NOT NULL AUTO_INCREMENT,
  `Salesman_id_FK` SMALLINT(5) UNSIGNED NOT NULL,
  `DateCreated` DATE NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `OUtgoingShipment_Salesman_Relationship_idx` (`Salesman_id_FK` ASC) VISIBLE,
  CONSTRAINT `OUtgoingShipment_Salesman_Relationship`
    FOREIGN KEY (`Salesman_id_FK`)
    REFERENCES `shambala`.`salesman` (`Id`)
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `shambala`.`scheme`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`scheme` (
  `Id` SMALLINT(5) UNSIGNED NOT NULL AUTO_INCREMENT,
  `Title` VARCHAR(60) NOT NULL,
  `DateCreated` DATE NOT NULL,
  `IsUserDefinedScheme` BIT(1) NOT NULL,
  `SchemeType` TINYINT(4) NOT NULL,
  `Value` DECIMAL(4,2) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `shambala`.`shop`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`shop` (
  `Title` VARCHAR(40) NOT NULL,
  `Address` VARCHAR(80) NULL DEFAULT NULL,
  `Scheme_Id_FK` SMALLINT(5) UNSIGNED NOT NULL,
  `Id` SMALLINT(5) UNSIGNED NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE,
  INDEX `Scheme_Id_FK_idx` (`Scheme_Id_FK` ASC) VISIBLE,
  CONSTRAINT `Shop_Scheme_Relationship`
    FOREIGN KEY (`Scheme_Id_FK`)
    REFERENCES `shambala`.`scheme` (`Id`)
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `shambala`.`invoice`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`invoice` (
  `Id` INT(10) UNSIGNED NOT NULL,
  `Outgoing_Shipment_Id_FK` INT(10) UNSIGNED NOT NULL,
  `Shop_Id_FK` SMALLINT(5) UNSIGNED NOT NULL,
  `Product_Id_FK` INT(10) UNSIGNED NOT NULL,
  `Flavour_Id_FK` TINYINT(4) UNSIGNED NOT NULL,
  `Scheme_Id_FK` SMALLINT(5) UNSIGNED NOT NULL,
  `CaretSize` TINYINT(4) NOT NULL,
  `QuantityPurchase` SMALLINT(5) UNSIGNED NOT NULL,
  `QuantityDefected` TINYINT(3) UNSIGNED NOT NULL,
  `GSTRate` TINYINT(4) NOT NULL,
  `CostPrice` DECIMAL(8,2) NOT NULL,
  `SellingPrice` DECIMAL(8,2) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE,
  INDEX `Shop_Invoice_Relationship_idx` (`Shop_Id_FK` ASC) VISIBLE,
  INDEX `Outgoing_Shipment_Invoice_Relationship_idx` (`Outgoing_Shipment_Id_FK` ASC) VISIBLE,
  INDEX `Scheme_Invoice_Relationship_idx` (`Scheme_Id_FK` ASC) VISIBLE,
  INDEX `Product_Invoice_Relationship_idx` (`Product_Id_FK` ASC) VISIBLE,
  INDEX `Flavour_Invoice_Relationship_idx` (`Flavour_Id_FK` ASC) VISIBLE,
  CONSTRAINT `Flavour_Invoice_Relationship`
    FOREIGN KEY (`Flavour_Id_FK`)
    REFERENCES `shambala`.`flavour` (`Id`),
  CONSTRAINT `Outgoing_Shipment_Invoice_Relationship`
    FOREIGN KEY (`Outgoing_Shipment_Id_FK`)
    REFERENCES `shambala`.`outgoing_shipment` (`Id`)
    ON UPDATE CASCADE,
  CONSTRAINT `Product_Invoice_Relationship`
    FOREIGN KEY (`Product_Id_FK`)
    REFERENCES `shambala`.`product` (`Id`)
    ON UPDATE CASCADE,
  CONSTRAINT `Scheme_Invoice_Relationship`
    FOREIGN KEY (`Scheme_Id_FK`)
    REFERENCES `shambala`.`scheme` (`Id`)
    ON UPDATE CASCADE,
  CONSTRAINT `Shop_Invoice_Relationship`
    FOREIGN KEY (`Shop_Id_FK`)
    REFERENCES `shambala`.`shop` (`Id`)
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `shambala`.`outgoing_shipment_details`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`outgoing_shipment_details` (
  `Id` INT(10) UNSIGNED NOT NULL,
  `Product_Id_FK` INT(11) UNSIGNED NOT NULL,
  `CaretSize` TINYINT(2) NOT NULL,
  `Total_Quantity_Shiped` SMALLINT(5) UNSIGNED NOT NULL,
  `Total_Quantity_Rejected` TINYINT(3) UNSIGNED NOT NULL,
  `Outgoing_Shipment_Id_FK` INT(10) UNSIGNED NOT NULL,
  `Flavour_Id_FK` TINYINT(4) UNSIGNED NOT NULL,
  `Status` CHAR(10) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE,
  INDEX `Outgoing_Shipment_Details_Product_RelationShip_idx` (`Product_Id_FK` ASC) VISIBLE,
  INDEX `Outgoing_Shipment_Details_Flavour_Relationship_idx` (`Flavour_Id_FK` ASC) VISIBLE,
  CONSTRAINT `Outgoing_Shipment_Details_Flavour_Relationship`
    FOREIGN KEY (`Flavour_Id_FK`)
    REFERENCES `shambala`.`flavour` (`Id`),
  CONSTRAINT `Outgoing_Shipment_Details_Product_RelationShip`
    FOREIGN KEY (`Product_Id_FK`)
    REFERENCES `shambala`.`product` (`Id`)
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


-- -----------------------------------------------------
-- Table `shambala`.`product_flavour_quantity`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`product_flavour_quantity` (
  `Id` TINYINT(4) NOT NULL,
  `Quantity` SMALLINT(6) UNSIGNED NOT NULL DEFAULT '0',
  `Flavour_Id_FK` TINYINT(4) UNSIGNED NOT NULL,
  `Product_Id_FK` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `Flavour_Id_FK_idx` (`Flavour_Id_FK` ASC) VISIBLE,
  INDEX `Product_Relationship_idx` (`Product_Id_FK` ASC) VISIBLE,
  CONSTRAINT `Product_Relationship`
    FOREIGN KEY (`Product_Id_FK`)
    REFERENCES `shambala`.`product` (`Id`)
    ON UPDATE CASCADE,
  CONSTRAINT `flavour_Relationship`
    FOREIGN KEY (`Flavour_Id_FK`)
    REFERENCES `shambala`.`flavour` (`Id`)
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
