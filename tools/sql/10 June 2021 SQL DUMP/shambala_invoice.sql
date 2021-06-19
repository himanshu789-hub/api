-- MySQL dump 10.13  Distrib 8.0.24, for Win64 (x86_64)
--
-- Host: localhost    Database: shambala
-- ------------------------------------------------------
-- Server version	8.0.20

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
-- Table structure for table `invoice`
--

DROP TABLE IF EXISTS `invoice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `invoice` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Outgoing_Shipment_Id_FK` int unsigned NOT NULL,
  `Shop_Id_FK` smallint unsigned NOT NULL,
  `Product_Id_FK` int unsigned NOT NULL,
  `Flavour_Id_FK` tinyint unsigned NOT NULL,
  `Scheme_Id_FK` smallint unsigned NOT NULL,
  `CaretSize` tinyint NOT NULL,
  `QuantityPurchase` smallint unsigned NOT NULL,
  `QuantityDefected` tinyint unsigned NOT NULL,
  `GSTRate` tinyint NOT NULL,
  `CostPrice` decimal(8,2) NOT NULL,
  `SellingPrice` decimal(8,2) NOT NULL,
  `DateCreated` date NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `Outgoing_Shipment_Invoice_Relationship_idx` (`Outgoing_Shipment_Id_FK`),
  KEY `Product_Invoice_Relationship_idx` (`Product_Id_FK`),
  KEY `Flavour_Invoice_Relationship_idx` (`Flavour_Id_FK`),
  KEY `Scheme_Invoice_Relationship_idx` (`Scheme_Id_FK`),
  KEY `Invoice_Shop_Relationship_idx` (`Shop_Id_FK`),
  CONSTRAINT `Flavour_Invoice_Relationship` FOREIGN KEY (`Flavour_Id_FK`) REFERENCES `flavour` (`Id`),
  CONSTRAINT `Invoice_Shop_Relationship` FOREIGN KEY (`Shop_Id_FK`) REFERENCES `shop` (`Id`),
  CONSTRAINT `Outgoing_Shipment_Invoice_Relationship` FOREIGN KEY (`Outgoing_Shipment_Id_FK`) REFERENCES `outgoing_shipment` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `Product_Invoice_Relationship` FOREIGN KEY (`Product_Id_FK`) REFERENCES `product` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `Scheme_Invoice_Relationship` FOREIGN KEY (`Scheme_Id_FK`) REFERENCES `scheme` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `invoice`
--

LOCK TABLES `invoice` WRITE;
/*!40000 ALTER TABLE `invoice` DISABLE KEYS */;
INSERT INTO `invoice` VALUES (4,1,1,3,1,1,30,10,0,0,1250.00,1125.00,'2021-05-13'),(6,3,1,1,3,1,0,6,0,0,5994.00,5394.60,'2021-05-13');
/*!40000 ALTER TABLE `invoice` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-06-10 21:08:21
