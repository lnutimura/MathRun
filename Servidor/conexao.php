<?php
    try{
        $conexao = new PDO('mysql:host=localhost;dbname=id334976_db_mathrun','id334976_db_mathrun','db_mathrun');
        $conexao->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        $conexao->exec("SET CHARACTER SET utf8");
    }catch(PDOException $e){
        echo 'ERROR: ' . $e->getMessage();
    }
?>