package com.ssafy.bugar.domain.insect.repository;

import com.ssafy.bugar.domain.insect.entity.CatchedInsect;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface CatchingInsectRepository extends JpaRepository<CatchedInsect, Long> {
}
