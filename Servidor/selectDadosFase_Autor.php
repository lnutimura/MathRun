<?php
    include("conexao.php");

    $pergunta = $_GET['id_fase'];

    $select = "SELECT f.id, f.nome as nome_fase, u.nome as nome_autor, u.data_nascimento, u.email
                FROM fase f, usuario u
                WHERE f.autor = u.id AND f.id =:pergunta";

    $result = $conexao->prepare($select);
    $result->bindParam(':pergunta', $pergunta, PDO::PARAM_STR); 
    $result->execute();
    $count = $result->rowCount(); 

    if($count > 0) {
        echo 1;
        foreach($result as $pergunta) {
            $id = $pergunta['id'];
            $nomeFase = $pergunta['nomeFase'];
            $nomeAutor = $pergunta['nomeAutor'];
            $data_nascimento = $pergunta['data_nascimento'];
            $email = $pergunta['email'];

            echo '&id='.$id.'#'.$nomeFase.'#'.$nomeAutor.'#'.$data_nascimento.'#'.$email;
        }
    }
    else{
        echo 0;
    }
?>