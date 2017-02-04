<?php
    include("conexao.php");

    $pergunta = $_GET['id_fase'];

    $select = "SELECT h.id, u.nome, h.acertou, h.data FROM historico h LEFT JOIN usuario u ON u.id = h.id_usuario WHERE h.id_fase =:pergunta ";

    $result = $conexao->prepare($select);
    $result->bindParam(':pergunta', $pergunta, PDO::PARAM_STR); 
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $pergunta) {
            $id = $pergunta['id'];
            $nome = $pergunta['nome'];
            $acertou = $pergunta['acertou'];
            $data = $pergunta['data'];

            echo '&id='.$id.'#'.$nome.'#'.$acertou.'#'.$data;
        }
    }
    else{
        echo 0;
    }
?>