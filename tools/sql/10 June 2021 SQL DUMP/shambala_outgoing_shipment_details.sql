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
-- Table structure for table `outgoing_shipment_details`
--

DROP TABLE IF EXISTS `outgoing_shipment_details`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `outgoing_shipment_details` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Product_Id_FK` int unsigned NOT NULL,
  `CaretSize` tinyint NOT NULL,
  `Total_Quantity_Shiped` smallint unsigned NOT NULL,
  `Total_Quantity_Rejected` tinyint unsigned NOT NULL,
  `Outgoing_Shipment_Id_FK` int unsigned NOT NULL,
  `Flavour_Id_FK` tinyint unsigned NOT NULL,
  `Total_Quantity_Returned` smallint unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `Outgoing_Shipment_Details_Product_RelationShip_idx` (`Product_Id_FK`),
  KEY `Outgoing_Shipment_Details_Flavour_Relationship_idx` (`Flavour_Id_FK`),
  KEY `Outgoing_Shipment_Details_OutgoingShipment_Relationship_idx` (`Outgoing_Shipment_Id_FK`),
  CONSTRAINT `Outgoing_Shipment_Details_Flavour_Relationship` FOREIGN KEY (`Flavour_Id_FK`) REFERENCES `flavour` (`Id`),
  CONSTRAINT `Outgoing_Shipment_Details_OutgoingShipment_Relationship` FOREIGN KEY (`Outgoing_Shipment_Id_FK`) REFERENCES `outgoing_shipment` (`Id`) ON UPDATE CASCADE,
  CONSTRAINT `Outgoing_Shipment_Details_Product_RelationShip` FOREIGN KEY (`Product_Id_FK`) REFERENCES `product` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=178 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `outgoing_shipment_details`
--

LOCK TABLES `outgoing_shipment_details` WRITE;
/*!40000 ALTER TABLE `outgoing_shipment_details` DISABLE KEYS */;
INSERT INTO `outgoing_shipment_details` VALUES (1,3,30,0,0,1,1,0),(2,3,30,60,2,2,1,0),(51,1,24,9,0,6,2,0),(177,1,24,6,0,3,3,0);
/*!40000 ALTER TABLE `outgoing_shipment_details` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-06-10 21:08:20
