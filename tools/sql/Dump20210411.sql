-- MySQL dump 10.13  Distrib 8.0.22, for Win64 (x86_64)
--
-- Host: localhost    Database: shambala
-- ------------------------------------------------------
-- Server version	8.0.4-rc-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `caret_detail`
--

DROP TABLE IF EXISTS `caret_detail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `caret_detail` (
  `Id` tinyint(3) unsigned NOT NULL,
  `CaretSize` tinyint(4) NOT NULL,
  `GSTRate` tinyint(4) NOT NULL,
  `CaretPrice` decimal(6,2) NOT NULL,
  `Product_Id_FK` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Product_Caret_Relationship_idx` (`Product_Id_FK`),
  CONSTRAINT `Product_Caret_Relationship` FOREIGN KEY (`Product_Id_FK`) REFERENCES `product` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `caret_detail`
--

LOCK TABLES `caret_detail` WRITE;
UNLOCK TABLES;

--
-- Table structure for table `flavour`
--

DROP TABLE IF EXISTS `flavour`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `flavour` (
  `Id` tinyint(4) unsigned NOT NULL AUTO_INCREMENT,
  `Title` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `flavour`
--

LOCK TABLES `flavour` WRITE;
INSERT INTO `flavour` VALUES (1,'Miranda'),(2,'Pepsi'),(3,'Mountain Dew'),(4,'7 UP'),(5,'Orange'),(6,'Apple'),(7,'Gauva');
UNLOCK TABLES;

--
-- Table structure for table `incoming_shipment`
--

DROP TABLE IF EXISTS `incoming_shipment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incoming_shipment` (
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Product_Id_FK` int(10) unsigned NOT NULL,
  `TotalRecievedPieces` smallint(5) unsigned DEFAULT NULL,
  `TotalDefectPieces` smallint(5) unsigned DEFAULT NULL,
  `CaretSize` tinyint(4) DEFAULT NULL,
  `DateCreated` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `IncomingShipment_Product_Relationship_idx` (`Product_Id_FK`),
  CONSTRAINT `IncomingShipment_Product_Relationship` FOREIGN KEY (`Product_Id_FK`) REFERENCES `product` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `incoming_shipment`
--

LOCK TABLES `incoming_shipment` WRITE;
UNLOCK TABLES;

--
-- Table structure for table `invoice`
--

DROP TABLE IF EXISTS `invoice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `invoice` (
  `Id` int(10) unsigned NOT NULL,
  `Outgoing_Shipment_Id_FK` int(10) unsigned NOT NULL,
  `Shop_Id_FK` smallint(5) unsigned NOT NULL,
  `Product_Id_FK` int(10) unsigned NOT NULL,
  `Flavour_Id_FK` tinyint(4) unsigned NOT NULL,
  `Scheme_Id_FK` smallint(5) unsigned NOT NULL,
  `CaretSize` tinyint(4) NOT NULL,
  `QuantityPurchase` smallint(5) unsigned NOT NULL,
  `QuantityDefected` tinyint(3) unsigned NOT NULL,
  `GSTRate` tinyint(4) NOT NULL,
  `CostPrice` decimal(8,2) NOT NULL,
  `SellingPrice` decimal(8,2) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `Shop_Invoice_Relationship_idx` (`Shop_Id_FK`),
  KEY `Outgoing_Shipment_Invoice_Relationship_idx` (`Outgoing_Shipment_Id_FK`),
  KEY `Scheme_Invoice_Relationship_idx` (`Scheme_Id_FK`),
  KEY `Product_Invoice_Relationship_idx` (`Product_Id_FK`),
  KEY `Flavour_Invoice_Relationship_idx` (`Flavour_Id_FK`),
  CONSTRAINT `Flavour_Invoice_Relationship` FOREIGN KEY (`Flavour_Id_FK`) REFERENCES `flavour` (`id`),
  CONSTRAINT `Outgoing_Shipment_Invoice_Relationship` FOREIGN KEY (`Outgoing_Shipment_Id_FK`) REFERENCES `outgoing_shipment` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `Product_Invoice_Relationship` FOREIGN KEY (`Product_Id_FK`) REFERENCES `product` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `Scheme_Invoice_Relationship` FOREIGN KEY (`Scheme_Id_FK`) REFERENCES `scheme` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `Shop_Invoice_Relationship` FOREIGN KEY (`Shop_Id_FK`) REFERENCES `shop` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `invoice`
--

LOCK TABLES `invoice` WRITE;
UNLOCK TABLES;

--
-- Table structure for table `outgoing_shipment`
--

DROP TABLE IF EXISTS `outgoing_shipment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `outgoing_shipment` (
  `Id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `Salesman_id_FK` smallint(5) unsigned NOT NULL,
  `DateCreated` date NOT NULL,
  `Status` char(10) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `OUtgoingShipment_Salesman_Relationship_idx` (`Salesman_id_FK`),
  CONSTRAINT `OUtgoingShipment_Salesman_Relationship` FOREIGN KEY (`Salesman_id_FK`) REFERENCES `salesman` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `outgoing_shipment`
--

LOCK TABLES `outgoing_shipment` WRITE;
UNLOCK TABLES;

--
-- Table structure for table `outgoing_shipment_details`
--

DROP TABLE IF EXISTS `outgoing_shipment_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `outgoing_shipment_details` (
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Product_Id_FK` int(11) unsigned NOT NULL,
  `CaretSize` tinyint(2) NOT NULL,
  `Total_Quantity_Shiped` smallint(5) unsigned NOT NULL,
  `Total_Quantity_Rejected` tinyint(3) unsigned NOT NULL,
  `Outgoing_Shipment_Id_FK` int(10) unsigned NOT NULL,
  `Flavour_Id_FK` tinyint(4) unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `Outgoing_Shipment_Details_Product_RelationShip_idx` (`Product_Id_FK`),
  KEY `Outgoing_Shipment_Details_Flavour_Relationship_idx` (`Flavour_Id_FK`),
  KEY `Outgoing_Shipment_Details_OutgoingShipment_Relationship_idx` (`Outgoing_Shipment_Id_FK`),
  CONSTRAINT `Outgoing_Shipment_Details_Flavour_Relationship` FOREIGN KEY (`Flavour_Id_FK`) REFERENCES `flavour` (`id`),
  CONSTRAINT `Outgoing_Shipment_Details_OutgoingShipment_Relationship` FOREIGN KEY (`Outgoing_Shipment_Id_FK`) REFERENCES `outgoing_shipment` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `Outgoing_Shipment_Details_Product_RelationShip` FOREIGN KEY (`Product_Id_FK`) REFERENCES `product` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `outgoing_shipment_details`
--

LOCK TABLES `outgoing_shipment_details` WRITE;
UNLOCK TABLES;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product` (
  `Name` varchar(20) NOT NULL,
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `CaretSize` tinyint(4) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product`
--

LOCK TABLES `product` WRITE;
INSERT INTO `product` VALUES ('200RGB',1,24),('300RGB',2,24),('300 Soda RGB',3,24),('250ML PET',4,30),('600ML PET',5,24),('750ML PET',6,24),('1.25L',7,12);
UNLOCK TABLES;

--
-- Table structure for table `product_flavour_quantity`
--

DROP TABLE IF EXISTS `product_flavour_quantity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product_flavour_quantity` (
  `Id` tinyint(4) NOT NULL AUTO_INCREMENT,
  `Quantity` smallint(6) unsigned NOT NULL DEFAULT '0',
  `Flavour_Id_FK` tinyint(4) unsigned NOT NULL,
  `Product_Id_FK` int(10) unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Flavour_Id_FK_idx` (`Flavour_Id_FK`),
  CONSTRAINT `flavour_Relationship` FOREIGN KEY (`Flavour_Id_FK`) REFERENCES `flavour` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product_flavour_quantity`
--

LOCK TABLES `product_flavour_quantity` WRITE;
INSERT INTO `product_flavour_quantity` VALUES (1,0,1,1),(2,0,2,1),(3,0,3,1),(4,0,4,1),(5,0,1,2),(6,0,2,2),(7,0,3,2),(8,0,4,2),(9,0,1,3),(10,0,2,3),(11,0,3,3),(12,0,4,3),(13,0,5,4),(14,0,6,4),(15,0,7,4),(16,0,5,5),(17,0,6,5),(18,0,7,5),(19,0,5,6),(20,0,6,6),(21,0,7,6),(22,0,5,7),(23,0,6,7),(24,0,7,7);
UNLOCK TABLES;

--
-- Table structure for table `salesman`
--

DROP TABLE IF EXISTS `salesman`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `salesman` (
  `Id` smallint(5) unsigned NOT NULL AUTO_INCREMENT,
  `FullName` varchar(60) NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salesman`
--

LOCK TABLES `salesman` WRITE;
INSERT INTO `salesman` VALUES (1,'John Doe',1),(2,'John Wick',1),(3,'Annie Marrie',1);
UNLOCK TABLES;

--
-- Table structure for table `scheme`
--

DROP TABLE IF EXISTS `scheme`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `scheme` (
  `Id` smallint(5) unsigned NOT NULL AUTO_INCREMENT,
  `Title` varchar(60) NOT NULL,
  `DateCreated` date NOT NULL,
  `IsUserDefinedScheme` bit(1) NOT NULL,
  `SchemeType` tinyint(4) NOT NULL,
  `Value` decimal(4,2) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scheme`
--

LOCK TABLES `scheme` WRITE;
UNLOCK TABLES;

--
-- Table structure for table `shop`
--

DROP TABLE IF EXISTS `shop`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `shop` (
  `Title` varchar(40) NOT NULL,
  `Address` varchar(80) DEFAULT NULL,
  `Scheme_Id_FK` smallint(5) unsigned NOT NULL,
  `Id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `Scheme_Id_FK_idx` (`Scheme_Id_FK`),
  CONSTRAINT `Shop_Scheme_Relationship` FOREIGN KEY (`Scheme_Id_FK`) REFERENCES `scheme` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shop`
--

LOCK TABLES `shop` WRITE;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-04-11 23:58:01
