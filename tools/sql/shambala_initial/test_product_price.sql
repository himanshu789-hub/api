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
INSERT INTO `product_price` VALUES (1,458.00,1,'0001-01-01'),(2,324.00,2,'0001-01-01'),(3,204.00,3,'0001-01-01'),(4,525.00,4,'0001-01-01'),(5,645.00,5,'0001-01-01'),(6,888.00,6,'0001-01-01'),(7,520.00,7,'0001-01-01'),(8,715.00,8,'0001-01-01'),(9,605.00,9,'0001-01-01'),(10,720.00,10,'0001-01-01'),(11,350.00,11,'0001-01-01'),(12,525.00,12,'0001-01-01'),(13,792.00,13,'0001-01-01'),(14,640.00,14,'0001-01-01'),(15,525.00,15,'0001-01-01'),(16,525.00,16,'0001-01-01'),(17,430.00,17,'0001-01-01'),(18,690.00,18,'0001-01-01'),(19,710.00,19,'0001-01-01'),(20,320.00,20,'0001-01-01'),(21,397.00,21,'0001-01-01'),(22,822.00,22,'0001-01-01'),(23,610.00,23,'0001-01-01'),(24,450.00,24,'0001-01-01'),(25,822.00,25,'0001-01-01'),(26,1050.00,26,'0001-01-01'),(27,1008.00,27,'0001-01-01'),(28,960.00,28,'0001-01-01'),(29,1056.00,29,'0001-01-01'),(30,165.00,30,'0001-01-01'),(31,231.00,31,'0001-01-01'),(32,145.00,32,'0001-01-01');
/*!40000 ALTER TABLE `product_price` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-10-08 21:45:32
