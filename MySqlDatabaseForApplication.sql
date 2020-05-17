-- phpMyAdmin SQL Dump
-- version 4.9.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Czas generowania: 17 Maj 2020, 15:30
-- Wersja serwera: 10.4.10-MariaDB
-- Wersja PHP: 7.3.12

SET FOREIGN_KEY_CHECKS=0;
SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Baza danych: `database`
--
CREATE DATABASE IF NOT EXISTS `database` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_polish_ci;
USE `database`;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `card_index`
--

DROP TABLE IF EXISTS `card_index`;
CREATE TABLE `card_index` (
  `id` int(11) NOT NULL,
  `file` varchar(6) COLLATE utf8_polish_ci NOT NULL,
  `name` varchar(50) COLLATE utf8_polish_ci NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `total_quantity_inventory` decimal(10,3) NOT NULL,
  `unit` varchar(15) COLLATE utf8_polish_ci NOT NULL,
  `id_provider_index` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `inventory`
--

DROP TABLE IF EXISTS `inventory`;
CREATE TABLE `inventory` (
  `id_inventory` int(11) NOT NULL,
  `income_inventory` decimal(10,3) NOT NULL,
  `expenditure_inventory` decimal(10,3) NOT NULL,
  `date_add_inventory` date NOT NULL,
  `id_ci_index` int(11) NOT NULL,
  `id_invoice_index` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_polish_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `invoice`
--

DROP TABLE IF EXISTS `invoice`;
CREATE TABLE `invoice` (
  `id_invoice` int(11) NOT NULL,
  `name_invoice` varchar(50) CHARACTER SET utf8 COLLATE utf8_polish_ci NOT NULL,
  `net_amount_invoice` decimal(10,2) NOT NULL,
  `gross_amount_invoice` decimal(10,2) NOT NULL,
  `date_invoice` date NOT NULL,
  `shortcut_name_invoice` varchar(50) CHARACTER SET utf8 COLLATE utf8_polish_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_polish_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `loginuser`
--

DROP TABLE IF EXISTS `loginuser`;
CREATE TABLE `loginuser` (
  `UserID` int(11) NOT NULL,
  `UserName` varchar(50) COLLATE utf8mb4_polish_ci NOT NULL,
  `Password` varchar(50) COLLATE utf8mb4_polish_ci NOT NULL,
  `user_type` varchar(50) COLLATE utf8mb4_polish_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_polish_ci;

--
-- Tabela Truncate przed wstawieniem `loginuser`
--

TRUNCATE TABLE `loginuser`;
--
-- Zrzut danych tabeli `loginuser`
--

INSERT INTO `loginuser` (`UserID`, `UserName`, `Password`, `user_type`) VALUES
(4, 'test2', 'ad0234829205b9033196ba818f7a872b', 'użytkownik'),
(5, 'admin', '21232f297a57a5a743894a0e4a801fc3', 'administrator'),
(6, 'test1', '5a105e8b9d40e1329780d62ea2265d8a', 'gość');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `provider`
--

DROP TABLE IF EXISTS `provider`;
CREATE TABLE `provider` (
  `id_provider` int(11) NOT NULL,
  `name_provider` varchar(50) COLLATE utf8_polish_ci NOT NULL,
  `nip` varchar(20) COLLATE utf8_polish_ci NOT NULL,
  `city` varchar(50) COLLATE utf8_polish_ci NOT NULL,
  `street` varchar(50) COLLATE utf8_polish_ci NOT NULL,
  `houseNumber` char(10) COLLATE utf8_polish_ci NOT NULL,
  `phoneNumber` char(15) COLLATE utf8_polish_ci NOT NULL,
  `bankAccount` char(30) COLLATE utf8_polish_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `card_index`
--
ALTER TABLE `card_index`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_provider_index` (`id_provider_index`);

--
-- Indeksy dla tabeli `inventory`
--
ALTER TABLE `inventory`
  ADD PRIMARY KEY (`id_inventory`),
  ADD KEY `id_ci_index` (`id_ci_index`),
  ADD KEY `id_invoice_index` (`id_invoice_index`);

--
-- Indeksy dla tabeli `invoice`
--
ALTER TABLE `invoice`
  ADD PRIMARY KEY (`id_invoice`);

--
-- Indeksy dla tabeli `loginuser`
--
ALTER TABLE `loginuser`
  ADD PRIMARY KEY (`UserID`),
  ADD UNIQUE KEY `UserName` (`UserName`);

--
-- Indeksy dla tabeli `provider`
--
ALTER TABLE `provider`
  ADD PRIMARY KEY (`id_provider`),
  ADD UNIQUE KEY `nip` (`nip`);

--
-- AUTO_INCREMENT dla tabel zrzutów
--

--
-- AUTO_INCREMENT dla tabeli `card_index`
--
ALTER TABLE `card_index`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT dla tabeli `inventory`
--
ALTER TABLE `inventory`
  MODIFY `id_inventory` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT dla tabeli `invoice`
--
ALTER TABLE `invoice`
  MODIFY `id_invoice` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT dla tabeli `loginuser`
--
ALTER TABLE `loginuser`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT dla tabeli `provider`
--
ALTER TABLE `provider`
  MODIFY `id_provider` int(11) NOT NULL AUTO_INCREMENT;

--
-- Ograniczenia dla zrzutów tabel
--

--
-- Ograniczenia dla tabeli `card_index`
--
ALTER TABLE `card_index`
  ADD CONSTRAINT `card_index_ibfk_1` FOREIGN KEY (`id_provider_index`) REFERENCES `provider` (`id_provider`);

--
-- Ograniczenia dla tabeli `inventory`
--
ALTER TABLE `inventory`
  ADD CONSTRAINT `inventory_ibfk_2` FOREIGN KEY (`id_invoice_index`) REFERENCES `invoice` (`id_invoice`),
  ADD CONSTRAINT `inventory_ibfk_3` FOREIGN KEY (`id_ci_index`) REFERENCES `card_index` (`id`);
SET FOREIGN_KEY_CHECKS=1;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
