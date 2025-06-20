<?php
session_start();

// Validaciones
if (!isset($_SESSION['idUsuario']) || !isset($_SESSION['rol']) || $_SESSION['rol'] !== 'Admin') {
    header("Location: ../login.php");
    exit();
}

require_once '../conexion.php';

// Conexión
$conn = Conexion::ConexionBD();

// Consultar solicitudes pendientes
$sql = "SELECT P.IdPrestamo, U.Nombre, U.Apellido, P.MontoPrestamo, P.PlazoMeses, P.Estado
        FROM Prestamos P
        INNER JOIN Clientes C ON P.IdCliente = C.IdCliente
        INNER JOIN Usuarios U ON C.IdUsuario = U.IdUsuario
        WHERE P.Estado = 'Pendiente'
        ORDER BY P.IdPrestamo DESC";

$stmt = $conn->query($sql);
$solicitudes = $stmt->fetchAll(PDO::FETCH_ASSOC);
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Solicitudes de Préstamo</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="bg-light">

<div class="container mt-5">
    <h2 class="text-center mb-4">Solicitudes Pendientes</h2>

    <?php if (!empty($solicitudes)): ?>
        <table class="table table-striped table-hover shadow">
            <thead class="table-primary">
                <tr>
                    <th>Cliente</th>
                    <th>Monto Solicitado</th>
                    <th>Plazo (meses)</th>
                    <th>Acciones</th>
                </tr>
            </thead>
            <tbody>
                <?php foreach ($solicitudes as $solicitud): ?>
                    <tr>
                        <td><?= htmlspecialchars($solicitud['Nombre'] . ' ' . $solicitud['Apellido']) ?></td>
                        <td>₡<?= number_format($solicitud['MontoPrestamo'], 2) ?></td>
                        <td><?= $solicitud['PlazoMeses'] ?></td>
                        <td>
                            <form action="validar_prestamo.php" method="POST" class="d-inline">
                                <input type="hidden" name="idPrestamo" value="<?= $solicitud['IdPrestamo'] ?>">
                                <button type="submit" name="accion" value="aceptar" class="btn btn-success btn-sm">Aceptar</button>
                            </form>
                            <form action="validar_prestamo.php" method="POST" class="d-inline">
                                <input type="hidden" name="idPrestamo" value="<?= $solicitud['IdPrestamo'] ?>">
                                <button type="submit" name="accion" value="rechazar" class="btn btn-danger btn-sm">Rechazar</button>
                            </form>
                        </td>
                    </tr>
                <?php endforeach; ?>
            </tbody>
        </table>

    <?php else: ?>
        <div class="alert alert-info text-center">
            No hay solicitudes pendientes en este momento.
        </div>
    <?php endif; ?>
</div>

</body>
</html>