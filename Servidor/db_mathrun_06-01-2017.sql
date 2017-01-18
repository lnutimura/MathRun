-- phpMyAdmin SQL Dump
-- version 4.6.5.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Jan 06, 2017 at 05:06 PM
-- Server version: 10.1.18-MariaDB
-- PHP Version: 7.0.8

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `id334976_db_mathrun`
--

-- --------------------------------------------------------

--
-- Table structure for table `casas`
--

CREATE TABLE `casas` (
  `id` int(11) NOT NULL,
  `id_fase` int(11) NOT NULL,
  `x` int(11) NOT NULL,
  `y` int(11) NOT NULL,
  `id_pergunta` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `casas`
--

INSERT INTO `casas` (`id`, `id_fase`, `x`, `y`, `id_pergunta`) VALUES
(1, 1, 1, 2, 1);

-- --------------------------------------------------------

--
-- Table structure for table `fase`
--

CREATE TABLE `fase` (
  `id` int(11) NOT NULL,
  `nome` varchar(60) NOT NULL,
  `autor` int(11) NOT NULL,
  `data` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `fase`
--

INSERT INTO `fase` (`id`, `nome`, `autor`, `data`) VALUES
(1, 'fase teste', 1, '2016-12-12');

-- --------------------------------------------------------

--
-- Table structure for table `historico`
--

CREATE TABLE `historico` (
  `id` int(11) NOT NULL,
  `id_usuario` int(11) NOT NULL,
  `id_fase` int(11) NOT NULL,
  `id_pergunta` int(11) NOT NULL,
  `resposta_dada` float NOT NULL,
  `acertou` int(11) NOT NULL,
  `data` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `perguntas`
--

CREATE TABLE `perguntas` (
  `id` int(11) NOT NULL,
  `questao` varchar(200) NOT NULL,
  `resposta` float NOT NULL,
  `dificuldade` int(11) NOT NULL,
  `tipo` int(11) NOT NULL,
  `autor` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `perguntas`
--

INSERT INTO `perguntas` (`id`, `questao`, `resposta`, `dificuldade`, `tipo`, `autor`) VALUES
(1, 'x = 1 + 1', 2, 1, 0, 1),
(2, 'Qual o valor de x em x = 4*3?', 12, 1, 2, 1),
(3, 'João possuia 10 laranjas, mas estava com fome e comeu 3, depois ele ganhou o dobro de laranjas do que ele ainda tinha e depois vendeu 6 para o mercado, com quantas laranjas ele ficou?', 15, 8, 4, 1);

-- --------------------------------------------------------

--
-- Table structure for table `usuario`
--

CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `login` varchar(50) NOT NULL,
  `senha` varchar(50) NOT NULL,
  `data_nascimento` date NOT NULL,
  `id_prof` int(11) NOT NULL DEFAULT '1',
  `is_prof` int(11) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `usuario`
--

INSERT INTO `usuario` (`id`, `nome`, `email`, `login`, `senha`, `data_nascimento`, `id_prof`, `is_prof`) VALUES
(1, 'Administrador', 'adm@adm.com', 'adm', 'adm', '2016-01-01', 1, 1),
(2, 'Raphael', 'raphasousa.jau@gmail.com', 'raphael', '123', '1995-10-23', 1, 0),
(3, 'Luan', 'luan@gmail.com', 'luanutimura', '123', '1996-09-04', 1, 0),
(4, 'Caio', 'caio@teste.com', 'caio', '123', '1994-03-03', 1, 0),
(5, 'Victor', 'victor@gmail.com', 'victor', '123', '1996-03-08', 1, 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `casas`
--
ALTER TABLE `casas`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_casa_fase` (`id_fase`),
  ADD KEY `fk_casa_pergunta` (`id_pergunta`);

--
-- Indexes for table `fase`
--
ALTER TABLE `fase`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_autor_fase` (`autor`);

--
-- Indexes for table `historico`
--
ALTER TABLE `historico`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_historico_usuario` (`id_usuario`),
  ADD KEY `fk_historico_pergunta` (`id_pergunta`),
  ADD KEY `fk_historico_fase` (`id_fase`);

--
-- Indexes for table `perguntas`
--
ALTER TABLE `perguntas`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_autor_pergunta` (`autor`);

--
-- Indexes for table `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `login` (`login`),
  ADD UNIQUE KEY `email` (`email`),
  ADD KEY `fk_prof_usuario` (`id_prof`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `casas`
--
ALTER TABLE `casas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT for table `fase`
--
ALTER TABLE `fase`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT for table `historico`
--
ALTER TABLE `historico`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `perguntas`
--
ALTER TABLE `perguntas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT for table `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `casas`
--
ALTER TABLE `casas`
  ADD CONSTRAINT `fk_casa_fase` FOREIGN KEY (`id_fase`) REFERENCES `fase` (`id`),
  ADD CONSTRAINT `fk_casa_pergunta` FOREIGN KEY (`id_pergunta`) REFERENCES `perguntas` (`id`);

--
-- Constraints for table `fase`
--
ALTER TABLE `fase`
  ADD CONSTRAINT `fk_autor_fase` FOREIGN KEY (`autor`) REFERENCES `usuario` (`id`);

--
-- Constraints for table `historico`
--
ALTER TABLE `historico`
  ADD CONSTRAINT `fk_historico_fase` FOREIGN KEY (`id_fase`) REFERENCES `fase` (`id`),
  ADD CONSTRAINT `fk_historico_pergunta` FOREIGN KEY (`id_pergunta`) REFERENCES `perguntas` (`id`),
  ADD CONSTRAINT `fk_historico_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id`);

--
-- Constraints for table `perguntas`
--
ALTER TABLE `perguntas`
  ADD CONSTRAINT `fk_autor_pergunta` FOREIGN KEY (`autor`) REFERENCES `usuario` (`id`);

--
-- Constraints for table `usuario`
--
ALTER TABLE `usuario`
  ADD CONSTRAINT `fk_prof_usuario` FOREIGN KEY (`id_prof`) REFERENCES `usuario` (`id`);

DELIMITER $$
--
-- Events
--
$$

DELIMITER ;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
