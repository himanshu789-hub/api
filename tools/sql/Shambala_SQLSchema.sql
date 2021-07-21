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
CREATE SCHEMA IF NOT EXISTS `shambala` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `shambala` ;

-- -----------------------------------------------------
-- Table `shambala`.`shop`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`shop` (
  `Title` VARCHAR(40) NOT NULL,
  `Address` VARCHAR(80) NULL DEFAULT NULL,
  `Id` SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 8
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `shambala`.`credit`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`credit` (
  `Id` INT NOT NULL,
  `Amount` DECIMAL(6,2) NULL DEFAULT NULL,
  `OutgoingShipment_Id_FK` INT UNSIGNED NOT NULL,
  `Shop_Id_FK` SMALLINT UNSIGNED NOT NULL,
  `DateCreated` DATE NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `Credit_Shop_Relationship_idx` (`Shop_Id_FK` ASC) VISIBLE,
  CONSTRAINT `Credit_Shop_Relationships`
    FOREIGN KEY (`Shop_Id_FK`)
    REFERENCES `shambala`.`shop` (`Id`)
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `shambala`.`flavour`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`flavour` (
  `Id` TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Title` VARCHAR(20) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 8
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `shambala`.`product`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`product` (
  `Name` VARCHAR(20) NOT NULL,
  `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `CaretSize` TINYINT NOT NULL,
  `PricePerCaret` DECIMAL(6,2) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 8
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `shambala`.`custom_carat_price`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`custom_carat_price` (
  `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Product_Id_FK` INT UNSIGNED NOT NULL,
  `Flavour_Id_FK` TINYINT UNSIGNED NOT NULL,
  `Quantity` SMALLINT UNSIGNED NOT NULL,
  `PricePerCarat` DECIMAL(6,2) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `Product_CustomCaratPrice_Relationship_idx` (`Product_Id_FK` ASC) VISIBLE,
  INDEX `Flavour_CustomCaratPriceRelationship` (`Flavour_Id_FK` ASC) VISIBLE,
  CONSTRAINT `Flavour_CustomCaratPriceRelationship`
    FOREIGN KEY (`Flavour_Id_FK`)
    REFERENCES `shambala`.`flavour` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `Product_CustomCaratPrice_Relationship`
    FOREIGN KEY (`Product_Id_FK`)
    REFERENCES `shambala`.`product` (`Id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `shambala`.`salesman`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`salesman` (
  `Id` SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `FullName` VARCHAR(60) NOT NULL,
  `IsActive` TINYINT(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `shambala`.`outgoing_shipment`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`outgoing_shipment` (
  `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Salesman_id_FK` SMALLINT UNSIGNED NOT NULL,
  `DateCreated` DATE NOT NULL,
  `Status` CHAR(10) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `OUtgoingShipment_Salesman_Relationship_idx` (`Salesman_id_FK` ASC) VISIBLE,
  CONSTRAINT `OUtgoingShipment_Salesman_Relationship`
    FOREIGN KEY (`Salesman_id_FK`)
    REFERENCES `shambala`.`salesman` (`Id`)
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 14
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `shambala`.`debit`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`debit` (
  `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `DateRecieved` DATE NOT NULL,
  `Amount` DECIMAL(6,2) NOT NULL,
  `Shop_Id_FK` SMALLINT UNSIGNED NOT NULL,
  `OutgoingShipment_Id_FK` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `Credit_OutgoingShipment_Relationship_idx` (`OutgoingShipment_Id_FK` ASC) VISIBLE,
  INDEX `Credit_Shop_Relationship_idx` (`Shop_Id_FK` ASC) VISIBLE,
  CONSTRAINT `Credit_OutgoingShipment_Relationship`
    FOREIGN KEY (`OutgoingShipment_Id_FK`)
    REFERENCES `shambala`.`outgoing_shipment` (`Id`),
  CONSTRAINT `Credit_Shop_Relationship`
    FOREIGN KEY (`Shop_Id_FK`)
    REFERENCES `shambala`.`shop` (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 6
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `shambala`.`incoming_shipment`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`incoming_shipment` (
  `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Product_Id_FK` INT UNSIGNED NOT NULL,
  `TotalRecievedPieces` SMALLINT UNSIGNED NULL DEFAULT NULL,
  `TotalDefectPieces` SMALLINT UNSIGNED NULL DEFAULT NULL,
  `CaretSize` TINYINT NULL DEFAULT NULL,
  `DateCreated` DATE NOT NULL,
  `Flavour_Id_FK` TINYINT UNSIGNED NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE,
  INDEX `IncomingShipment_Product_Relationship_idx` (`Product_Id_FK` ASC) VISIBLE,
  INDEX `IncmingShipment_Flavour_idx` (`Flavour_Id_FK` ASC) VISIBLE,
  CONSTRAINT `IncmingShipment_Flavour`
    FOREIGN KEY (`Flavour_Id_FK`)
    REFERENCES `shambala`.`flavour` (`Id`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT,
  CONSTRAINT `IncomingShipment_Product_Relationship`
    FOREIGN KEY (`Product_Id_FK`)
    REFERENCES `shambala`.`product` (`Id`)
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 980
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `shambala`.`outgoing_shipment_details`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`outgoing_shipment_details` (
  `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Product_Id_FK` INT UNSIGNED NOT NULL,
  `CaretSize` TINYINT NOT NULL,
  `Total_Quantity_Shiped` SMALLINT UNSIGNED NOT NULL,
  `Total_Quantity_Rejected` TINYINT UNSIGNED NOT NULL,
  `Outgoing_Shipment_Id_FK` INT UNSIGNED NOT NULL,
  `Flavour_Id_FK` TINYINT UNSIGNED NOT NULL,
  `Total_Quantity_Returned` SMALLINT UNSIGNED NOT NULL,
  `Scheme_Total_Quantity` TINYINT NOT NULL,
  `Scheme_Total_Price` DECIMAL(6,2) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE,
  INDEX `Outgoing_Shipment_Details_Product_RelationShip_idx` (`Product_Id_FK` ASC) VISIBLE,
  INDEX `Outgoing_Shipment_Details_Flavour_Relationship_idx` (`Flavour_Id_FK` ASC) VISIBLE,
  INDEX `Outgoing_Shipment_Details_OutgoingShipment_Relationship_idx` (`Outgoing_Shipment_Id_FK` ASC) VISIBLE,
  CONSTRAINT `Outgoing_Shipment_Details_Flavour_Relationship`
    FOREIGN KEY (`Flavour_Id_FK`)
    REFERENCES `shambala`.`flavour` (`Id`),
  CONSTRAINT `Outgoing_Shipment_Details_OutgoingShipment_Relationship`
    FOREIGN KEY (`Outgoing_Shipment_Id_FK`)
    REFERENCES `shambala`.`outgoing_shipment` (`Id`)
    ON UPDATE CASCADE,
  CONSTRAINT `Outgoing_Shipment_Details_Product_RelationShip`
    FOREIGN KEY (`Product_Id_FK`)
    REFERENCES `shambala`.`product` (`Id`)
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 884
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `shambala`.`product_flavour_quantity`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`product_flavour_quantity` (
  `Id` TINYINT NOT NULL AUTO_INCREMENT,
  `Quantity` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
  `Flavour_Id_FK` TINYINT UNSIGNED NOT NULL,
  `Product_Id_FK` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `Flavour_Id_FK_idx` (`Flavour_Id_FK` ASC) VISIBLE,
  INDEX `Product_Relationship_idx` (`Product_Id_FK` ASC) VISIBLE,
  CONSTRAINT `flavour_Relationship`
    FOREIGN KEY (`Flavour_Id_FK`)
    REFERENCES `shambala`.`flavour` (`Id`)
    ON UPDATE CASCADE,
  CONSTRAINT `Product_Relationship`
    FOREIGN KEY (`Product_Id_FK`)
    REFERENCES `shambala`.`product` (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 25
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `shambala`.`scheme`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `shambala`.`scheme` (
  `Id` SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `DateCreated` DATE NOT NULL,
  `Quantity` TINYINT NULL DEFAULT NULL,
  `Product_Id_FK` INT NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Id_UNIQUE` (`Id` ASC) VISIBLE,
  UNIQUE INDEX `Product_Id_FK_UNIQUE` (`Product_Id_FK` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 788
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
