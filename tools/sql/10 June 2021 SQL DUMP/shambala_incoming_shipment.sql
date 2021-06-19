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
-- Table structure for table `incoming_shipment`
--

DROP TABLE IF EXISTS `incoming_shipment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incoming_shipment` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Product_Id_FK` int unsigned NOT NULL,
  `TotalRecievedPieces` smallint unsigned DEFAULT NULL,
  `TotalDefectPieces` smallint unsigned DEFAULT NULL,
  `CaretSize` tinyint DEFAULT NULL,
  `DateCreated` datetime NOT NULL,
  `Flavour_Id_FK` tinyint unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `IncomingShipment_Product_Relationship_idx` (`Product_Id_FK`),
  KEY `IncmingShipment_Flavour_idx` (`Flavour_Id_FK`),
  CONSTRAINT `IncmingShipment_Flavour` FOREIGN KEY (`Flavour_Id_FK`) REFERENCES `flavour` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `IncomingShipment_Product_Relationship` FOREIGN KEY (`Product_Id_FK`) REFERENCES `product` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=980 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `incoming_shipment`
--

LOCK TABLES `incoming_shipment` WRITE;
/*!40000 ALTER TABLE `incoming_shipment` DISABLE KEYS */;
INSERT INTO `incoming_shipment` VALUES (1,1,190,9,12,'0001-01-01 00:00:00',4),(2,1,190,9,12,'0001-01-01 00:00:00',4),(3,1,190,9,12,'0001-01-01 00:00:00',4),(4,1,190,1,12,'0001-01-01 00:00:00',4),(106,1,0,0,24,'0001-01-01 00:00:00',1),(126,2,0,0,24,'0001-01-01 00:00:00',1),(850,2,5,0,24,'0001-01-01 00:00:00',1),(872,1,198,0,24,'0001-01-01 00:00:00',2),(979,2,0,0,24,'0001-01-01 00:00:00',3);
/*!40000 ALTER TABLE `incoming_shipment` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-06-10 21:08:17
