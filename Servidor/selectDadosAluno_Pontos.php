<?php
    include("conexao.php");

    $pergunta = $_GET['id_usuario'];

    $select = "SELECT h.id, h.acertou, h.data, p.tipo
                FROM historico h
                LEFT JOIN perguntas p
                ON p.id = h.id_pergunta
                WHERE h.id_usuario =:pergunta";

    $result = $conexao->prepare($select);
    $result->bindParam(':pergunta', $pergunta, PDO::PARAM_STR); 
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $pergunta) {
            $id = $pergunta['id'];
            $acertou = $pergunta['acertou'];
            $data = $pergunta['data'];
            $tipo = $pergunta['tipo'];

            echo '&id='.$id.'#'.$acertou.'#'.$data.'#'.$tipo;
        }
    }
    else{
        echo 0;
    }
?>