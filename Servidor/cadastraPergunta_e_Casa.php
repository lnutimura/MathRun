<?php
    include("conexao.php");

    $questao = $_GET['questao']; 			//string 200
    $resposta = $_GET['resposta']; 			//float
    $dificuldade = $_GET['dificuldade']; 	//int
    $tipo = $_GET['tipo']; 					//int
    $autor = $_GET['autor']; 				//int FK usuario (id)

    $x = $_GET['x'];
    $y = $_GET['y'];

    //salva no banco a pergunta
    $select = "INSERT INTO perguntas (questao, resposta, dificuldade, tipo, autor) VALUES (:questao, :resposta, :dificuldade, :tipo, :autor)";
    $result = $conexao->prepare($select);
    $result->bindParam(':questao', $questao, PDO::PARAM_STR); 
    $result->bindParam(':resposta', $resposta, PDO::PARAM_STR);
    $result->bindParam(':dificuldade', $dificuldade, PDO::PARAM_STR);
    $result->bindParam(':tipo', $tipo, PDO::PARAM_STR);
    $result->bindParam(':autor', $autor, PDO::PARAM_STR);
    $result->execute();

    if ($result) {
        //pega a ultima pergunta cadastrada acima
        $select2 = "SELECT id FROM perguntas WHERE questao = :questao AND resposta = :resposta AND dificuldade = :dificuldade AND tipo = :tipo AND autor = :autor";
        $result2 = $conexao->prepare($select2);
        $result2->bindParam(':questao', $questao, PDO::PARAM_STR); 
        $result2->bindParam(':resposta', $resposta, PDO::PARAM_STR);
        $result2->bindParam(':dificuldade', $dificuldade, PDO::PARAM_STR);
        $result2->bindParam(':tipo', $tipo, PDO::PARAM_STR);
        $result2->bindParam(':autor', $autor, PDO::PARAM_STR);
        $result2->execute();

        if ($result2) {
            foreach($result2 as $res2) {
                $pergunta = $res2['id'];
            }

            //pega a ultima fase cadastrada do autor
            $select1 = "SELECT id FROM fase WHERE autor = :autor ORDER BY id DESC LIMIT 1";
            $result1 = $conexao->prepare($select1);
            $result1->bindParam(':autor', $autor, PDO::PARAM_STR);
            $result1->execute();

            if ($result1) {
                foreach($result1 as $res1) {
                    $fase = $res1['id'];
                }
                //cadastra a casa
                $select3 = "INSERT INTO casas (id_fase, x, y, id_pergunta) VALUES (:fase, :x, :y, :pergunta)";
                $result3 = $conexao->prepare($select3);
                $result3->bindParam(':fase', $fase, PDO::PARAM_STR); 
                $result3->bindParam(':x', $x, PDO::PARAM_STR);
                $result3->bindParam(':y', $y, PDO::PARAM_STR);
                $result3->bindParam(':pergunta', $pergunta, PDO::PARAM_STR);
                $result3->execute();
                if ($result3) {
                    echo 1;
                }else{
                    echo 0;
                } 

            }else{
                echo 0;
            }
        }else{
            echo 0;
        }
    }else{
        echo 0;
    }
?>