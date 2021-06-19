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
-- Table structure for table `product_flavour_quantity`
--

DROP TABLE IF EXISTS `product_flavour_quantity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product_flavour_quantity` (
  `Id` tinyint NOT NULL AUTO_INCREMENT,
  `Quantity` smallint unsigned NOT NULL DEFAULT '0',
  `Flavour_Id_FK` tinyint unsigned NOT NULL,
  `Product_Id_FK` int unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Flavour_Id_FK_idx` (`Flavour_Id_FK`),
  CONSTRAINT `flavour_Relationship` FOREIGN KEY (`Flavour_Id_FK`) REFERENCES `flavour` (`Id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product_flavour_quantity`
--

LOCK TABLES `product_flavour_quantity` WRITE;
/*!40000 ALTER TABLE `product_flavour_quantity` DISABLE KEYS */;
INSERT INTO `product_flavour_quantity` VALUES (1,0,1,1),(2,189,2,1),(3,0,3,1),(4,370,4,1),(5,5,1,2),(6,0,2,2),(7,0,3,2),(8,0,4,2),(9,10,1,3),(10,0,2,3),(11,0,3,3),(12,0,4,3),(13,0,5,4),(14,0,6,4),(15,0,7,4),(16,0,5,5),(17,0,6,5),(18,0,7,5),(19,0,5,6),(20,0,6,6),(21,0,7,6),(22,0,5,7),(23,0,6,7),(24,0,7,7);
/*!40000 ALTER TABLE `product_flavour_quantity` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-06-10 21:08:19
