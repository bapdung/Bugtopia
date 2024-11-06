package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.entity.Egg;
import java.util.List;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface EggRepository extends JpaRepository<Egg, Long>{
    List<Egg> findByUserIdOrderByCreatedDateDesc(Long userId);
}
