-- MySQL dump 10.13  Distrib 8.0.24, for Win64 (x86_64)
--
-- Host: localhost    Database: shambala
-- ------------------------------------------------------
-- Server version	8.0.20

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `credit`
--

DROP TABLE IF EXISTS `credit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `credit` (
  `Id` int NOT NULL,
  `Amount` decimal(6,2) DEFAULT NULL,
  `OutgoingShipment_Id_FK` int unsigned NOT NULL,
  `Shop_Id_FK` smallint unsigned NOT NULL,
  `DateCreated` date NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Credit_Shop_Relationship_idx` (`Shop_Id_FK`),
  CONSTRAINT `Credit_Shop_Relationships` FOREIGN KEY (`Shop_Id_FK`) REFERENCES `shop` (`Id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `credit`
--

LOCK TABLES `credit` WRITE;
/*!40000 ALTER TABLE `credit` DISABLE KEYS */;
/*!40000 ALTER TABLE `credit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `custom_carat_price`
--

DROP TABLE IF EXISTS `custom_carat_price`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `custom_carat_price` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Quantity` smallint unsigned NOT NULL,
  `PricePerCarat` decimal(6,2) NOT NULL,
  `OutgoinShipmentDetail_Id_FK` int unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `OutgoingShipment_CustomPrice_Relationship_idx` (`OutgoinShipmentDetail_Id_FK`),
  CONSTRAINT `OutgoingShipment_CustomPrice_Relationship` FOREIGN KEY (`OutgoinShipmentDetail_Id_FK`) REFERENCES `outgoing_shipment_details` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `custom_carat_price`
--

LOCK TABLES `custom_carat_price` WRITE;
/*!40000 ALTER TABLE `custom_carat_price` DISABLE KEYS */;
/*!40000 ALTER TABLE `custom_carat_price` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `debit`
--

DROP TABLE IF EXISTS `debit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `debit` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `DateRecieved` date NOT NULL,
  `Amount` decimal(6,2) NOT NULL,
  `Shop_Id_FK` smallint unsigned NOT NULL,
  `OutgoingShipment_Id_FK` int unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Credit_OutgoingShipment_Relationship_idx` (`OutgoingShipment_Id_FK`),
  KEY `Credit_Shop_Relationship_idx` (`Shop_Id_FK`),
  CONSTRAINT `Credit_OutgoingShipment_Relationship` FOREIGN KEY (`OutgoingShipment_Id_FK`) REFERENCES `outgoing_shipment` (`Id`),
  CONSTRAINT `Credit_Shop_Relationship` FOREIGN KEY (`Shop_Id_FK`) REFERENCES `shop` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debit`
--

LOCK TABLES `debit` WRITE;
/*!40000 ALTER TABLE `debit` DISABLE KEYS */;
INSERT INTO `debit` VALUES (1,'2021-06-23',100.00,1,1),(2,'2021-06-23',10.00,1,1),(3,'2021-06-23',1000.00,1,3),(4,'0001-01-01',110.00,1,1),(5,'0001-01-01',11.00,7,1);
/*!40000 ALTER TABLE `debit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `flavour`
--

DROP TABLE IF EXISTS `flavour`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `flavour` (
  `Id` tinyint unsigned NOT NULL AUTO_INCREMENT,
  `Title` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `flavour`
--

LOCK TABLES `flavour` WRITE;
/*!40000 ALTER TABLE `flavour` DISABLE KEYS */;
INSERT INTO `flavour` VALUES (1,'Miranda'),(2,'Pepsi'),(3,'Mountain Dew'),(4,'7 UP'),(5,'Orange'),(6,'Apple'),(7,'Gauva');
/*!40000 ALTER TABLE `flavour` ENABLE KEYS */;
UNLOCK TABLES;

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
  `DateCreated` date NOT NULL,
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
INSERT INTO `incoming_shipment` VALUES (1,1,190,9,12,'0001-01-01',4),(2,1,190,9,12,'0001-01-01',4),(3,1,190,9,12,'0001-01-01',4),(4,1,190,1,12,'0001-01-01',4),(8,2,240,0,24,'0001-01-01',1),(58,1,1200,0,24,'0001-01-01',3),(106,1,0,0,24,'0001-01-01',1),(126,2,0,0,24,'0001-01-01',1),(470,2,1200,0,24,'0001-01-01',3),(616,1,1200,0,24,'0001-01-01',2),(697,1,568,0,24,'0001-01-01',1),(818,2,1200,0,24,'0001-01-01',2),(850,2,5,0,24,'0001-01-01',1),(872,1,198,0,24,'0001-01-01',2),(979,2,0,0,24,'0001-01-01',3);
/*!40000 ALTER TABLE `incoming_shipment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `outgoing_shipment`
--

DROP TABLE IF EXISTS `outgoing_shipment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `outgoing_shipment` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Salesman_id_FK` smallint unsigned NOT NULL,
  `DateCreated` date NOT NULL,
  `Status` char(10) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `OUtgoingShipment_Salesman_Relationship_idx` (`Salesman_id_FK`),
  CONSTRAINT `OUtgoingShipment_Salesman_Relationship` FOREIGN KEY (`Salesman_id_FK`) REFERENCES `salesman` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `outgoing_shipment`
--

LOCK TABLES `outgoing_shipment` WRITE;
/*!40000 ALTER TABLE `outgoing_shipment` DISABLE KEYS */;
INSERT INTO `outgoing_shipment` VALUES (1,1,'2021-05-13','COMPLETED'),(2,1,'2021-05-13','RETURN'),(3,2,'2021-05-13','COMPLETED'),(6,3,'2021-05-15','COMPLETED'),(7,3,'2021-06-23','PENDING'),(8,1,'2021-06-25','RETURN'),(9,1,'2021-06-26','RETURN'),(10,1,'2021-06-26','COMPLETED'),(11,3,'2021-06-27','RETURN'),(12,1,'2021-07-08','PENDING'),(13,1,'2021-07-08','RETURN');
/*!40000 ALTER TABLE `outgoing_shipment` ENABLE KEYS */;
UNLOCK TABLES;

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
  `Scheme_Total_Quantity` tinyint NOT NULL,
  `Scheme_Total_Price` decimal(6,2) NOT NULL,
  `Price_Per_Carat` decimal(6,2) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `Outgoing_Shipment_Details_Product_RelationShip_idx` (`Product_Id_FK`),
  KEY `Outgoing_Shipment_Details_Flavour_Relationship_idx` (`Flavour_Id_FK`),
  KEY `Outgoing_Shipment_Details_OutgoingShipment_Relationship_idx` (`Outgoing_Shipment_Id_FK`),
  CONSTRAINT `Outgoing_Shipment_Details_Flavour_Relationship` FOREIGN KEY (`Flavour_Id_FK`) REFERENCES `flavour` (`Id`),
  CONSTRAINT `Outgoing_Shipment_Details_OutgoingShipment_Relationship` FOREIGN KEY (`Outgoing_Shipment_Id_FK`) REFERENCES `outgoing_shipment` (`Id`) ON UPDATE CASCADE,
  CONSTRAINT `Outgoing_Shipment_Details_Product_RelationShip` FOREIGN KEY (`Product_Id_FK`) REFERENCES `product` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=884 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `outgoing_shipment_details`
--

LOCK TABLES `outgoing_shipment_details` WRITE;
/*!40000 ALTER TABLE `outgoing_shipment_details` DISABLE KEYS */;
INSERT INTO `outgoing_shipment_details` VALUES (1,3,24,24,0,1,1,0,0,0.00,NULL),(2,3,30,60,2,2,1,0,0,0.00,NULL),(51,1,24,9,0,6,2,0,0,0.00,NULL),(177,1,24,6,0,3,3,0,0,0.00,NULL),(264,1,24,240,0,10,3,0,0,0.00,NULL),(367,1,24,27,0,12,1,0,0,0.00,NULL),(379,2,24,720,0,11,3,0,0,0.00,NULL),(391,1,24,240,0,8,1,0,0,0.00,NULL),(448,3,24,24,0,13,1,0,0,0.00,NULL),(712,1,24,240,0,9,2,0,0,0.00,NULL),(877,1,24,12,0,13,1,0,0,0.00,NULL),(883,1,24,1,0,13,2,0,0,0.00,NULL);
/*!40000 ALTER TABLE `outgoing_shipment_details` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product` (
  `Name` varchar(20) NOT NULL,
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `CaretSize` tinyint NOT NULL,
  `PricePerCaret` decimal(6,2) NOT NULL,
  `SchemeQuantity` tinyint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product`
--

LOCK TABLES `product` WRITE;
/*!40000 ALTER TABLE `product` DISABLE KEYS */;
INSERT INTO `product` VALUES ('200RGB',1,24,999.00,1),('300RGB',2,24,586.00,2),('300 Soda RGB',3,24,125.00,3),('250ML PET',4,30,366.00,4),('600ML PET',5,24,1200.00,5),('750ML PET',6,24,1420.00,1),('1.25L',7,12,692.00,2);
/*!40000 ALTER TABLE `product` ENABLE KEYS */;
UNLOCK TABLES;

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
  KEY `Product_Relationship_idx` (`Product_Id_FK`),
  CONSTRAINT `flavour_Relationship` FOREIGN KEY (`Flavour_Id_FK`) REFERENCES `flavour` (`Id`) ON UPDATE CASCADE,
  CONSTRAINT `Product_Relationship` FOREIGN KEY (`Product_Id_FK`) REFERENCES `product` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product_flavour_quantity`
--

LOCK TABLES `product_flavour_quantity` WRITE;
/*!40000 ALTER TABLE `product_flavour_quantity` DISABLE KEYS */;
INSERT INTO `product_flavour_quantity` VALUES (1,289,1,1),(2,1196,2,1),(3,1008,3,1),(4,370,4,1),(5,245,1,2),(6,1200,2,2),(7,720,3,2),(8,0,4,2),(9,17,1,3),(10,0,2,3),(11,0,3,3),(12,0,4,3),(13,0,5,4),(14,0,6,4),(15,0,7,4),(16,0,5,5),(17,0,6,5),(18,0,7,5),(19,0,5,6),(20,0,6,6),(21,0,7,6),(22,0,5,7),(23,0,6,7),(24,0,7,7);
/*!40000 ALTER TABLE `product_flavour_quantity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `salesman`
--

DROP TABLE IF EXISTS `salesman`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `salesman` (
  `Id` smallint unsigned NOT NULL AUTO_INCREMENT,
  `FullName` varchar(60) NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salesman`
--

LOCK TABLES `salesman` WRITE;
/*!40000 ALTER TABLE `salesman` DISABLE KEYS */;
INSERT INTO `salesman` VALUES (1,'John Doe',1),(2,'John Beas',1),(3,'Annie Marrie',1),(4,'Pattrick Beas',0),(5,'Pattrick Beas',0),(6,'Pattrick Beas',1);
/*!40000 ALTER TABLE `salesman` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `scheme`
--

DROP TABLE IF EXISTS `scheme`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `scheme` (
  `Id` smallint unsigned NOT NULL AUTO_INCREMENT,
  `DateCreated` date NOT NULL,
  `Quantity` tinyint DEFAULT NULL,
  `Product_Id_FK` int unsigned NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  KEY `Scheme_Product_Relationship_idx` (`Product_Id_FK`),
  CONSTRAINT `Scheme_Product_Relationship` FOREIGN KEY (`Product_Id_FK`) REFERENCES `product` (`Id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=788 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scheme`
--

LOCK TABLES `scheme` WRITE;
/*!40000 ALTER TABLE `scheme` DISABLE KEYS */;
INSERT INTO `scheme` VALUES (1,'0001-01-01',2,1),(782,'0001-01-01',2,2),(783,'2021-07-11',2,3),(784,'2021-07-11',4,4),(785,'2021-07-11',3,5),(786,'2021-07-11',2,7),(787,'2121-07-11',1,6);
/*!40000 ALTER TABLE `scheme` ENABLE KEYS */;
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
  `Id` smallint unsigned NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `shop`
--

LOCK TABLES `shop` WRITE;
/*!40000 ALTER TABLE `shop` DISABLE KEYS */;
INSERT INTO `shop` VALUES ('General Store','Tellibandha No.7',1),('sds','sas',5),('Shoe','Shankar',6),('Aib','sdsda',7);
/*!40000 ALTER TABLE `shop` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'shambala'
--

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed
