-- MySQL dump 10.13  Distrib 8.0.26, for Win64 (x86_64)
--
-- Host: localhost    Database: test
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
-- Table structure for table `product_flavour_quantity`
--

DROP TABLE IF EXISTS `product_flavour_quantity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product_flavour_quantity` (
  `Id` tinyint(4) NOT NULL AUTO_INCREMENT,
  `Quantity` smallint(5) unsigned NOT NULL DEFAULT '0',
  `Flavour_Id_FK` tinyint(3) unsigned NOT NULL,
  `Product_Id_FK` int(10) unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Flavour_Id_FK_idx` (`Flavour_Id_FK`),
  KEY `Product_Relationship_idx` (`Product_Id_FK`),
  CONSTRAINT `Product_Relationship` FOREIGN KEY (`Product_Id_FK`) REFERENCES `product` (`id`),
  CONSTRAINT `flavour_Relationship` FOREIGN KEY (`Flavour_Id_FK`) REFERENCES `flavour` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=125 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product_flavour_quantity`
--

LOCK TABLES `product_flavour_quantity` WRITE;
/*!40000 ALTER TABLE `product_flavour_quantity` DISABLE KEYS */;
INSERT INTO `product_flavour_quantity` VALUES (1,0,9,1),(2,0,10,1),(3,0,11,1),(4,0,12,1),(5,0,19,1),(6,0,9,2),(7,0,10,2),(8,0,11,2),(9,0,12,2),(10,0,19,2),(11,0,21,3),(12,0,9,4),(13,0,10,4),(14,0,11,4),(15,0,12,4),(16,0,9,5),(17,0,10,5),(18,0,11,5),(19,0,12,5),(20,0,9,6),(21,0,10,6),(22,0,11,6),(23,0,12,6),(24,0,9,7),(25,0,10,7),(26,0,11,7),(27,0,12,7),(28,0,9,8),(29,0,10,8),(30,0,11,8),(31,0,12,8),(32,0,9,9),(33,0,10,9),(34,0,11,9),(35,0,12,9),(36,0,13,9),(37,0,18,9),(38,0,9,10),(39,0,10,10),(40,0,11,10),(41,0,12,10),(42,0,13,10),(43,0,18,10),(44,0,21,11),(45,0,21,12),(46,0,21,13),(47,0,21,14),(48,0,21,15),(49,0,21,16),(50,0,21,17),(51,0,21,18),(52,0,21,19),(53,0,21,20),(54,0,1,21),(55,0,22,21),(56,0,7,21),(57,0,4,21),(58,0,5,21),(59,0,7,22),(60,0,4,22),(61,0,5,22),(62,0,2,22),(63,0,15,23),(64,0,8,23),(65,0,16,23),(66,0,1,24),(67,0,8,24),(68,0,7,24),(69,0,2,24),(70,0,5,24),(71,0,22,24),(72,0,3,24),(73,0,4,24),(74,0,1,25),(75,0,7,25),(76,0,2,25),(77,0,4,25),(78,0,5,25),(79,0,21,26),(80,0,22,27),(81,0,6,28),(82,0,1,28),(83,0,8,28),(84,0,7,28),(85,0,2,28),(86,0,5,28),(87,0,22,28),(88,0,3,28),(89,0,4,28),(90,0,6,29),(91,0,1,29),(92,0,8,29),(93,0,7,29),(94,0,2,29),(95,0,5,29),(96,0,22,29),(97,0,3,29),(98,0,4,29),(99,0,21,30),(100,0,21,31),(101,0,21,32);
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

-- Dump completed on 2021-10-08 21:45:31
