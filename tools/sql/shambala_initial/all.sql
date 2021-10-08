CREATE DATABASE  IF NOT EXISTS `test` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;
USE `test`;
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
-- Table structure for table `flavour`
--

DROP TABLE IF EXISTS `flavour`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `flavour` (
  `Id` tinyint(3) unsigned NOT NULL AUTO_INCREMENT,
  `Title` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `flavour`
--

LOCK TABLES `flavour` WRITE;
/*!40000 ALTER TABLE `flavour` DISABLE KEYS */;
INSERT INTO `flavour` (`Id`, `Title`) VALUES (1,'Orange'),(2,'Guava'),(3,'Pineapple'),(4,'Pomegranate'),(5,'Lychee'),(6,'Cranberry'),(7,'Mix Fruit'),(8,'Mango'),(9,'Pepsi'),(10,'Miranda'),(11,'Mountain Dew'),(12,'7UP'),(13,'Black Pepsi'),(14,'Blue Pepsi'),(15,'Chocolate'),(16,'Coffee'),(17,'Black Pepsi'),(18,'Diet'),(19,'Slice'),(20,'MIXTURE'),(21,'N/F'),(22,'Apple');
/*!40000 ALTER TABLE `flavour` ENABLE KEYS */;
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
  `PricePerCaret` decimal(6,2) NOT NULL,
  `SchemeQuantity` tinyint(4) unsigned DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product`
--

LOCK TABLES `product` WRITE;
/*!40000 ALTER TABLE `product` DISABLE KEYS */;
INSERT INTO `product` (`Name`, `Id`, `CaretSize`, `PricePerCaret`, `SchemeQuantity`) VALUES ('200ML RGB',1,24,248.00,0),('300ML RGB',2,24,324.00,0),('SD 300',3,24,204.00,0),('250ML CD',4,30,525.00,0),('600ML CD',5,24,645.00,0),('750ML CD',6,24,888.00,0),('1.25L CD',7,12,520.00,0),('2.25L CD',8,9,715.00,0),('250ML CAN',9,24,605.00,0),('330ML CAN',10,24,720.00,0),('750ML SODA',11,24,350.00,0),('250ML SLICE',12,30,525.00,0),('600ML SLICE',13,24,792.00,0),('1.25L SLICE',14,12,640.00,0),('Nimboo Masala',15,30,525.00,0),('Nimboo',16,30,525.00,0),('Lipton Tea',17,24,430.00,0),('Sting Can',18,24,690.00,0),('1.75L SLICE',19,9,710.00,0),('1.25 SL TP',20,40,320.00,0),('TC 200 PET',21,24,397.00,0),('TC 500 PET',22,24,822.00,0),('200ML VAD',23,24,610.00,0),('200ML TP',24,30,450.00,0),('1L PET',25,12,822.00,0),('500ML Getoret',26,24,1050.00,0),('1L TP 105',27,12,1008.00,0),('1L TP 100',28,12,960.00,0),('1L TP 110',29,12,1056.00,0),('250AQ',30,30,165.00,0),('500ML AQ',31,30,231.00,0),('1L AQ',32,12,145.00,0);
/*!40000 ALTER TABLE `product` ENABLE KEYS */;
UNLOCK TABLES;

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
INSERT INTO `product_flavour_quantity` (`Id`, `Quantity`, `Flavour_Id_FK`, `Product_Id_FK`) VALUES (1,0,9,1),(2,0,10,1),(3,0,11,1),(4,0,12,1),(5,0,19,1),(6,0,9,2),(7,0,10,2),(8,0,11,2),(9,0,12,2),(10,0,19,2),(11,0,21,3),(12,0,9,4),(13,0,10,4),(14,0,11,4),(15,0,12,4),(16,0,9,5),(17,0,10,5),(18,0,11,5),(19,0,12,5),(20,0,9,6),(21,0,10,6),(22,0,11,6),(23,0,12,6),(24,0,9,7),(25,0,10,7),(26,0,11,7),(27,0,12,7),(28,0,9,8),(29,0,10,8),(30,0,11,8),(31,0,12,8),(32,0,9,9),(33,0,10,9),(34,0,11,9),(35,0,12,9),(36,0,13,9),(37,0,18,9),(38,0,9,10),(39,0,10,10),(40,0,11,10),(41,0,12,10),(42,0,13,10),(43,0,18,10),(44,0,21,11),(45,0,21,12),(46,0,21,13),(47,0,21,14),(48,0,21,15),(49,0,21,16),(50,0,21,17),(51,0,21,18),(52,0,21,19),(53,0,21,20),(54,0,1,21),(55,0,22,21),(56,0,7,21),(57,0,4,21),(58,0,5,21),(59,0,7,22),(60,0,4,22),(61,0,5,22),(62,0,2,22),(63,0,15,23),(64,0,8,23),(65,0,16,23),(66,0,1,24),(67,0,8,24),(68,0,7,24),(69,0,2,24),(70,0,5,24),(71,0,22,24),(72,0,3,24),(73,0,4,24),(74,0,1,25),(75,0,7,25),(76,0,2,25),(77,0,4,25),(78,0,5,25),(79,0,21,26),(80,0,22,27),(81,0,6,28),(82,0,1,28),(83,0,8,28),(84,0,7,28),(85,0,2,28),(86,0,5,28),(87,0,22,28),(88,0,3,28),(89,0,4,28),(90,0,6,29),(91,0,1,29),(92,0,8,29),(93,0,7,29),(94,0,2,29),(95,0,5,29),(96,0,22,29),(97,0,3,29),(98,0,4,29),(99,0,21,30),(100,0,21,31),(101,0,21,32);
/*!40000 ALTER TABLE `product_flavour_quantity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product_price`
--

DROP TABLE IF EXISTS `product_price`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product_price` (
  `Id` mediumint(9) NOT NULL AUTO_INCREMENT,
  `Price` decimal(7,2) DEFAULT NULL,
  `Product_Id_FK` int(10) unsigned NOT NULL,
  `Date_Created` date NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Price_product_Relationship_idx` (`Product_Id_FK`),
  CONSTRAINT `Price_product_Relationship` FOREIGN KEY (`Product_Id_FK`) REFERENCES `product` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product_price`
--

LOCK TABLES `product_price` WRITE;
/*!40000 ALTER TABLE `product_price` DISABLE KEYS */;
INSERT INTO `product_price` (`Id`, `Price`, `Product_Id_FK`, `Date_Created`) VALUES (1,458.00,1,'0001-01-01'),(2,324.00,2,'0001-01-01'),(3,204.00,3,'0001-01-01'),(4,525.00,4,'0001-01-01'),(5,645.00,5,'0001-01-01'),(6,888.00,6,'0001-01-01'),(7,520.00,7,'0001-01-01'),(8,715.00,8,'0001-01-01'),(9,605.00,9,'0001-01-01'),(10,720.00,10,'0001-01-01'),(11,350.00,11,'0001-01-01'),(12,525.00,12,'0001-01-01'),(13,792.00,13,'0001-01-01'),(14,640.00,14,'0001-01-01'),(15,525.00,15,'0001-01-01'),(16,525.00,16,'0001-01-01'),(17,430.00,17,'0001-01-01'),(18,690.00,18,'0001-01-01'),(19,710.00,19,'0001-01-01'),(20,320.00,20,'0001-01-01'),(21,397.00,21,'0001-01-01'),(22,822.00,22,'0001-01-01'),(23,610.00,23,'0001-01-01'),(24,450.00,24,'0001-01-01'),(25,822.00,25,'0001-01-01'),(26,1050.00,26,'0001-01-01'),(27,1008.00,27,'0001-01-01'),(28,960.00,28,'0001-01-01'),(29,1056.00,29,'0001-01-01'),(30,165.00,30,'0001-01-01'),(31,231.00,31,'0001-01-01'),(32,145.00,32,'0001-01-01');
/*!40000 ALTER TABLE `product_price` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `scheme`
--

DROP TABLE IF EXISTS `scheme`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `scheme` (
  `Id` smallint(5) NOT NULL AUTO_INCREMENT,
  `DateCreated` date NOT NULL,
  `Quantity` tinyint(4) unsigned NOT NULL,
  `Product_Id_FK` int(10) unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `Scheme_Product_Relationship_idx` (`Product_Id_FK`),
  CONSTRAINT `Scheme_Product_Relationship` FOREIGN KEY (`Product_Id_FK`) REFERENCES `product` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scheme`
--

LOCK TABLES `scheme` WRITE;
/*!40000 ALTER TABLE `scheme` DISABLE KEYS */;
INSERT INTO `scheme` (`Id`, `DateCreated`, `Quantity`, `Product_Id_FK`) VALUES (1,'0001-01-01',0,1),(2,'0001-01-01',0,2),(3,'0001-01-01',0,3),(4,'0001-01-01',0,4),(5,'0001-01-01',0,5),(6,'0001-01-01',0,6),(7,'0001-01-01',0,7),(8,'0001-01-01',0,8),(9,'0001-01-01',0,9),(10,'0001-01-01',0,10),(11,'0001-01-01',0,11),(12,'0001-01-01',0,12),(13,'0001-01-01',0,13),(14,'0001-01-01',0,14),(15,'0001-01-01',0,15),(16,'0001-01-01',0,16),(17,'0001-01-01',0,17),(18,'0001-01-01',0,18),(19,'0001-01-01',0,19),(20,'0001-01-01',0,20),(21,'0001-01-01',0,21),(22,'0001-01-01',0,22),(23,'0001-01-01',0,23),(24,'0001-01-01',0,24),(25,'0001-01-01',0,25),(26,'0001-01-01',0,26),(27,'0001-01-01',0,27),(28,'0001-01-01',0,28),(29,'0001-01-01',0,29),(30,'0001-01-01',0,30),(31,'0001-01-01',0,31),(32,'0001-01-01',0,32);
/*!40000 ALTER TABLE `scheme` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-10-08 21:48:44
