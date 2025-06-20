<?php
session_start();
$idCliente = $_SESSION['idCliente']; // o ajustá según cómo guardás el ID

// Llamar a la API
$url = "http://localhost:7230/api/Reportes/prestamos-activos?idCliente=$idCliente";
$response = file_get_contents($url);
$prestamos = json_decode($response, true);
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Préstamos Activos</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="bg-light">
    <div class="container py-5">
        <h2 class="mb-4">📋 Préstamos Activos</h2>
        <?php if (!empty($prestamos)) : ?>
            <table class="table table-bordered table-striped">
                <thead class="table-dark">
                    <tr>
                        <th>ID</th>
                        <th>Monto</th>
                        <th>Estado</th>
                    </tr>
                </thead>
                <tbody>
                    <?php foreach ($prestamos as $p) : ?>
                        <tr>
                            <td><?= $p['idPrestamo'] ?></td>
                            <td>₡<?= number_format($p['monto'], 2) ?></td>
                            <td><?= $p['estado'] ?></td>
                        </tr>
                    <?php endforeach ?>
                </tbody>
            </table>
        <?php else : ?>
            <div class="alert alert-warning">No tienes préstamos activos en este momento.</div>
        <?php endif ?>
    </div>
</body>
</html>