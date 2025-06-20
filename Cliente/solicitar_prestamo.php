<?php
session_start();
if (!isset($_SESSION['idUsuario'])) {
    header("Location: ../login.php");
    exit();
}
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Solicitud de Préstamo</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="bg-light">

<div class="container mt-5">
    <h2 class="text-center mb-4">Solicitud de Préstamo</h2>

    <?php if (isset($_SESSION['mensaje'])): ?>
        <div class="alert alert-<?=$_SESSION['tipo']?>">
            <?= $_SESSION['mensaje'] ?>
        </div>
        <?php unset($_SESSION['mensaje'], $_SESSION['tipo']); ?>
    <?php endif; ?>

    <form action="procesar_formulario.php" method="POST" class="bg-white p-4 shadow rounded">

        <div class="mb-3">
            <label for="monto" class="form-label">Monto del préstamo (colones)</label>
            <input type="number" name="monto" id="monto" class="form-control" min="100000" max="100000000" required>
        </div>

        <div class="mb-3">
            <label for="plazo" class="form-label">Plazo en meses</label>
            <input type="number" name="plazo" id="plazo" class="form-control" min="1" max="120" required>
        </div>

        <button type="submit" class="btn btn-primary w-100">Enviar solicitud</button>
    </form>
</div>

</body>
</html>